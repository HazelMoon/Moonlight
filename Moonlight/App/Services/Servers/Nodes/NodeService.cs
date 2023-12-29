using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Exceptions.Server;
using Moonlight.App.Extensions;
using Moonlight.App.Helpers;
using Moonlight.App.Models.Servers.Api.Resources;
using Moonlight.App.Repositories;

namespace Moonlight.App.Services.Servers.Nodes;

public class NodeService
{
    private readonly IServiceProvider ServiceProvider;

    public NodeBootService Boot => ServiceProvider.GetRequiredService<NodeBootService>();
    public NodeNetworkingService Networking => ServiceProvider.GetRequiredService<NodeNetworkingService>();

    public NodeService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async Task<Status> GetStatus(ServerNode node)
    {
        using var client = node.CreateHttpClient();
        
        var status = await client.SendHandled<Status, NodeException>(HttpMethod.Get, "status");

        return status;
    }
}