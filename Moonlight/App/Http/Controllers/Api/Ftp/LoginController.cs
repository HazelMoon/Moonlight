using Microsoft.AspNetCore.Mvc;
using Moonlight.App.Database.Entities;
using Moonlight.App.Database.Enums;
using Moonlight.App.Helpers;
using Moonlight.App.Http.Requests.Ftp;
using Moonlight.App.Models.Abstractions.Services;
using Moonlight.App.Models.Abstractions.Services.Extensions;
using Moonlight.App.Models.Enums;
using Moonlight.App.Repositories;
using Moonlight.App.Services.ServiceManage;
using Moonlight.App.Services.Utils;

namespace Moonlight.App.Http.Controllers.Api.Ftp;

[ApiController]
[Route("api/ftp/login")]
public class LoginController : Controller
{
    private readonly IServiceProvider ServiceProvider;
    private readonly ServiceService ServiceService;
    private readonly JwtService JwtService;

    public LoginController(IServiceProvider serviceProvider, ServiceService serviceService, JwtService jwtService)
    {
        ServiceProvider = serviceProvider;
        ServiceService = serviceService;
        JwtService = jwtService;
    }
    
    public async Task<ActionResult> Login([FromBody] Login login)
    {
        // Validate resource type
        if (!Enum.TryParse(login.ResourceType, true, out ServiceType type))
            return BadRequest();

        ServiceDefinition definition;
        
        // Check for definition
        try
        {
            definition = ServiceService.Definition.Get(type);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        
        // Check if definition supports ftp actions
        if (definition.Actions is not IServiceFtpActions ftpActions)
            return BadRequest();

        // Check if the password is a jwt
        if (login.Password.StartsWith("ey"))
        {
            if (await TryJwtLogin(login))
                return Ok();
        }
        
        // Search for user
        var userRepo = ServiceProvider.GetRequiredService<Repository<User>>();
        var user = userRepo
            .Get()
            .FirstOrDefault(x => x.Username == login.Username);

        if (user == null)
            return StatusCode(403);
        
        if (!HashHelper.Verify(login.Password, user.Password))
        {
            Logger.Warn($"A failed login attempt via ftp has occured. Username: '{login.Username}', Resource Id: '{login.ResourceId}', Resource Type: {login.ResourceType}");
            return StatusCode(403);
        }

        if (await ftpActions.AuthenticateFtpLogin(ServiceProvider, user, login.ResourceId, Request))
            return Ok();

        return StatusCode(403);
    }

    private async Task<bool> TryJwtLogin(Login login)
    {
        if (!await JwtService.Validate(login.Password, JwtType.FtpLogin))
            return false;
        
        var data = await JwtService.Decode(login.Password);

        if (!data.ContainsKey("ResourceId"))
            return false;

        if (!int.TryParse(data["ResourceId"], out int resourceId))
            return false;

        return login.ResourceId == resourceId;
    }
}