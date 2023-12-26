using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Models.Enums;
using Moonlight.App.Packets.Servers.Client;
using Moonlight.App.Repositories;
using Moonlight.App.Services.Servers.Nodes;

namespace Moonlight.App.Services.Servers;

public class ServerPowerService
{
    private readonly IServiceProvider ServiceProvider;
    private readonly NodeService NodeService;

    public ServerPowerService(IServiceProvider serviceProvider, NodeService nodeService)
    {
        ServiceProvider = serviceProvider;
        NodeService = nodeService;
    }

    public async Task PerformPowerAction(Server s, PowerAction powerAction)
    {
        using var scope = ServiceProvider.CreateScope();
        var serverRepo = scope.ServiceProvider.GetRequiredService<Repository<Server>>();
        
        var server = serverRepo
            .Get()
            .Include(x => x.Node)
            .First(x => x.Id == s.Id);

        await NodeService.Networking.SendWsPacket(server.Node, new ServerPowerAction()
        {
            Id = server.Id,
            Action = powerAction
        });
    }
}