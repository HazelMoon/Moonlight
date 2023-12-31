using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Helpers;
using Moonlight.App.Http.Requests.Ftp;
using Moonlight.App.Models.Abstractions;
using Moonlight.App.Models.Enums;
using Moonlight.App.Repositories;
using Moonlight.App.Services;
using Moonlight.App.Services.Utils;

namespace Moonlight.App.Http.Controllers.Api;

[ApiController]
[Route("api/ftp")]
public class FtpController : Controller
{
    private readonly IServiceProvider ServiceProvider;

    public FtpController(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] Login login)
    {
        switch (login.ResourceType)
        {
            case "server":
                return await LoginServer(login);
            default:
                return StatusCode(403);
        }
    }

    private async Task<ActionResult> LoginServer(Login login)
    {
        var requestHelper = ServiceProvider.GetRequiredService<NodeRequestHelper>();
        await requestHelper.UnpackNode(this);

        // JWT Login for panel file manager
        if (login.Password.StartsWith("ey")) // Check for a jwt
        {
            var jwtService = ServiceProvider.GetRequiredService<JwtService>();

            if (await jwtService.Validate(login.Password, JwtType.FtpServer)) // Check if its actually a valid jwt
            {
                var data = await jwtService.Decode(login.Password);

                if (data.ContainsKey("ServerId")) // Check if the server id is included
                {
                    var serverId = int.Parse(data["ServerId"]);

                    if (serverId == login.ResourceId) // Allow login if its the same id
                        return Ok();
                }
            }
        }

        var userRepo = ServiceProvider.GetRequiredService<Repository<User>>();
        var user = userRepo
            .Get()
            .FirstOrDefault(x => x.Username == login.Username);

        if (user == null)
            return StatusCode(403);

        var serverRepo = ServiceProvider.GetRequiredService<Repository<Server>>();
        var server = serverRepo
            .Get()
            .Include(x => x.Service)
            .ThenInclude(x => x.Owner)
            .Include(x => x.Service)
            .ThenInclude(x => x.Shares)
            .ThenInclude(x => x.User)
            .FirstOrDefault(x => x.Id == login.ResourceId);

        if (server == null)
            return StatusCode(403);

        if (!HashHelper.Verify(login.Password, user.Password))
        {
            Logger.Warn($"A failed login attempt via ftp has occured. Username: '{login.Username}', Resource Id: '{login.ResourceId}', Resource Type: {login.ResourceType}");
            return StatusCode(403);
        }

        var permissionStorage = new PermissionStorage(user.Permissions);

        if (permissionStorage[Permission.AdminServers])
            return Ok();

        if (user.Id == server.Service.Owner.Id)
            return Ok();

        if (server.Service.Shares.Any(x => x.User.Id == user.Id))
            return Ok();

        return StatusCode(403);
    }
}