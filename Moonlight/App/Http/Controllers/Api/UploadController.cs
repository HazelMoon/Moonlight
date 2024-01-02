using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities.Store;
using Moonlight.App.Helpers;
using Moonlight.App.Models.Abstractions.Services;
using Moonlight.App.Models.Abstractions.Services.Extensions;
using Moonlight.App.Models.Enums;
using Moonlight.App.Repositories;
using Moonlight.App.Services;
using Moonlight.App.Services.ServiceManage;
using Moonlight.App.Services.Utils;

namespace Moonlight.App.Http.Controllers.Api;

[ApiController]
[Route("api/upload")]
public class UploadController : Controller
{
    private readonly IdentityService IdentityService;
    private readonly JwtService JwtService;
    private readonly ServiceService ServiceService;
    private readonly Repository<Service> ServiceRepository;
    private readonly IServiceProvider ServiceProvider;
    
    public UploadController(
        IdentityService identityService,
        JwtService jwtService,
        ServiceService serviceService,
        Repository<Service> serviceRepository,
        IServiceProvider serviceProvider)
    {
        IdentityService = identityService;
        JwtService = jwtService;
        ServiceService = serviceService;
        ServiceRepository = serviceRepository;
        ServiceProvider = serviceProvider;
    }
    
    // The following method/api endpoint needs some explanation:
    // Because of blazor and dropzone.js, we need an api endpoint
    // to upload files via the built in file manager.
    // As we learned from user experiences in v1b
    // a large data transfer via the signal r connection might lead to
    // failing uploads for some users with a unstable connection. That's
    // why we implement this api endpoint. It can potentially prevent
    // upload from malicious scripts as well. To verify the user is
    // authenticated and to know where to place the file, we use 2 jwts.
    // One jwt is the user's token. We will authenticate it using the
    // identity service and check if the user has access to the service in
    // the first place. Then we have a separate jwt which specifies what
    // server we want to upload to and where to place the files. This jwts
    // will be generated every 4 minutes in the file manager upload section
    // and only last 5 minutes.


    [HttpPost]
    public async Task<ActionResult> Upload([FromQuery(Name = "token")] string uploadToken)
    {
        // Check if a file exist and if it is not too big
        if (!Request.Form.Files.Any())
            return BadRequest();

        if (ByteSizeValue.FromBytes(Request.Form.Files.First().Length).MegaBytes > 100)
            return BadRequest("File too large");
        
        // Validate request
        await IdentityService.Authenticate(Request);

        if (!IdentityService.IsSignedIn)
            return StatusCode(403);

        if (!await JwtService.Validate(uploadToken, JwtType.FileUpload))
            return StatusCode(403);

        var uploadData = await JwtService.Decode(uploadToken);

        if (!uploadData.ContainsKey("ServiceId") || !uploadData.ContainsKey("Path"))
            return BadRequest();

        if (!int.TryParse(uploadData["ServiceId"], out int serviceId))
            return BadRequest();

        var service = ServiceRepository
            .Get()
            .Include(x => x.Product)
            .FirstOrDefault(x => x.Id == serviceId);

        if (service == null)
            return BadRequest();

        // Load service definition and check if it supports file upload
        ServiceDefinition definition;
        
        try
        {
            definition = ServiceService.Definition.Get(service.Product.Type);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        
        if (definition.Actions is not IServiceFileManagerActions fileManagerActions) // => Service implementation does not support file upload
            return BadRequest();

        // Upload the file
        var file = Request.Form.Files.First();

        if (await fileManagerActions.ProcessFileUpload(
                ServiceProvider,
                IdentityService.CurrentUser,
                service,
                file.Name,
                file.OpenReadStream()))
        {
            return Ok();
        }

        return Problem();
    }
}