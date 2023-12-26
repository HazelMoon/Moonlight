using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Packets.Servers.Client;
using Moonlight.App.Repositories;
using Moonlight.App.Services.Servers.Nodes;

namespace Moonlight.App.Services.Servers;

public class ServerConsoleService
{
    private readonly IServiceProvider ServiceProvider;
    private readonly NodeService NodeService;

    public ServerConsoleService(IServiceProvider serviceProvider, NodeService nodeService)
    {
        ServiceProvider = serviceProvider;
        NodeService = nodeService;
    }

    public async Task Subscribe(Server s)
    {
        using var scope = ServiceProvider.CreateScope();
        var serverRepo = scope.ServiceProvider.GetRequiredService<Repository<Server>>();
        
        var server = serverRepo
            .Get()
            .Include(x => x.Node)
            .First(x => x.Id == s.Id);

        await NodeService.Networking.SendWsPacket(server.Node, new ServerConsoleSubscribe()
        {
            Id = server.Id
        });
    }
}