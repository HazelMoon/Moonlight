using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Exceptions;
using Moonlight.App.Exceptions.Server;
using Moonlight.App.Extensions;
using Moonlight.App.Helpers;
using Moonlight.App.Http.Resources.Servers;
using Moonlight.App.Repositories;

namespace Moonlight.App.Services.Servers.Nodes;

public class NodeBootService
{
    private readonly NodeService NodeService;
    private readonly IServiceProvider ServiceProvider;

    public NodeBootService(NodeService nodeService, IServiceProvider serviceProvider)
    {
        NodeService = nodeService;
        ServiceProvider = serviceProvider;
    }
    
    public Task BootAll() // This method will boot all nodes async
    {
        Logger.Info("Booting all nodes");
        
        using var scope = ServiceProvider.CreateScope();
        var nodeRepo = scope.ServiceProvider.GetRequiredService<Repository<ServerNode>>();
        
        foreach (var node in nodeRepo.Get().ToArray())
        {
            Task.Run(async () =>
            {
                try
                {
                    await Boot(node);
                }
                catch (Exception e)
                {
                    Logger.Fatal("Unhandled error while booting node in BootAll");
                    Logger.Fatal(e);
                }
            });
        }
        
        return Task.CompletedTask;
    }

    public async Task Boot(ServerNode node)
    {
        var status = await NodeService.GetStatus(node);

        if (status.IsBooting)
            throw new DisplayException("Unable to boot now, as the node is already booting");

        await Start(node);

        // Load servers
        using var scope = ServiceProvider.CreateScope();
        var serverRepo = scope.ServiceProvider.GetRequiredService<Repository<Server>>();

        var servers = serverRepo
            .Get()
            .Where(x => x.Node.Id == node.Id)
            .Include(x => x.Variables)
            .Include(x => x.MainAllocation)
            .Include(x => x.Allocations)
            .Include(x => x.Image)
            .ThenInclude(x => x.DockerImages)
            .Include(x => x.Image)
            .ThenInclude(x => x.Variables)
            .ToArray();
        
        var serverConfigurations = new List<ServerConfiguration>();
        
        foreach (var server in servers)
            serverConfigurations.Add(server.ToServerConfiguration());

        foreach (var chunk in serverConfigurations.Chunk(50))
            await SendServers(node, chunk);
        
        await Restore(node);
    }

    public async Task Start(ServerNode node)
    {
        using var client = node.CreateHttpClient();
        await client.SendHandled<NodeException>(HttpMethod.Post, "boot");
    }

    public async Task SendServers(ServerNode node, ServerConfiguration[] serverConfigurations)
    {
        using var client = node.CreateHttpClient();
        await client.SendHandled<NodeException>(HttpMethod.Post, "boot/servers", serverConfigurations);
    }

    public async Task Restore(ServerNode node)
    {
        using var client = node.CreateHttpClient();
        await client.SendHandled<NodeException>(HttpMethod.Post, "boot/restore");
    }
}