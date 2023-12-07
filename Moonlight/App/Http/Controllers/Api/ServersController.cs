using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Exceptions.Api;
using Moonlight.App.Extensions;
using Moonlight.App.Helpers;
using Moonlight.App.Http.Resources.Servers;
using Moonlight.App.Repositories;
using Moonlight.App.Services.Servers.Nodes;

namespace Moonlight.App.Http.Controllers.Api;

[ApiController]
[Route("api/servers")]
public class ServersController : Controller
{
    private readonly NodeRequestHelper RequestHelper;
    private readonly Repository<Server> ServerRepository;
    private readonly NodeService NodeService;

    public ServersController(NodeRequestHelper requestHelper, Repository<Server> serverRepository, NodeService nodeService)
    {
        RequestHelper = requestHelper;
        ServerRepository = serverRepository;
        NodeService = nodeService;
    }

    [HttpPost]
    [Route("boot")]
    public async Task<ActionResult> Boot() // This function triggers the boot process for a node
    {
        var node = await RequestHelper.UnpackNode(this);
        
        Logger.Info($"Boot signal received from node '{node.Name}'");

        try
        {
            await NodeService.Boot.Boot(node);
            
            Logger.Info($"Successfully started booting for node '{node.Name}'");
        }
        catch (Exception e)
        {
            Logger.Fatal($"An unknown error occured while booting node '{node.Name}' via remote boot signal");
            Logger.Fatal(e);
            throw;
        }

        return Ok();
    }

    [HttpGet("install/{id:int}")]
    public async Task<ActionResult<ServerInstallConfiguration>> GetInstall(int id)
    {
        await RequestHelper.UnpackNode(this);

        var server = ServerRepository
            .Get()
            .Include(x => x.Image)
            .FirstOrDefault(x => x.Id == id);

        if (server == null)
            throw new NotFoundException("A server with this id cannot be found");

        return Ok(server.ToServerInstallConfiguration());
    }
}