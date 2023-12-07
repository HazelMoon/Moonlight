using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Exceptions.Server;
using Moonlight.App.Extensions;
using Moonlight.App.Models.Server.Api.Resources;

namespace Moonlight.App.Services.Servers.Nodes;

public class NodeService
{
    private readonly IServiceProvider ServiceProvider;

    public NodeBootService Boot => ServiceProvider.GetRequiredService<NodeBootService>();

    public NodeService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async Task<Status> GetStatus(ServerNode node)
    {
        using var client = node.CreateHttpClient();
        
        var status = await client.SendHandled<Status, NodeException>(HttpMethod.Get, "status", headers: headers =>
        {
            headers.Add("Authorization", node.Token);
        });

        return status;
    }
}