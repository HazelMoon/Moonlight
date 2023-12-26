using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Exceptions.Server;
using Moonlight.App.Extensions;
using Moonlight.App.Repositories;

namespace Moonlight.App.Services.Servers;

public class ServerService
{
    private readonly IServiceProvider ServiceProvider;

    public ServerMetaService Meta => ServiceProvider.GetRequiredService<ServerMetaService>();
    public ServerPowerService Power => ServiceProvider.GetRequiredService<ServerPowerService>();
    public ServerConsoleService Console => ServiceProvider.GetRequiredService<ServerConsoleService>();

    public ServerService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async Task Sync(Server s)
    {
        using var scope = ServiceProvider.CreateScope();
        var serverRepo = scope.ServiceProvider.GetRequiredService<Repository<Server>>();

        var server = serverRepo
            .Get()
            .Include(x => x.Node)
            .Include(x => x.Variables)
            .Include(x => x.MainAllocation)
            .Include(x => x.Allocations)
            .Include(x => x.Image)
            .ThenInclude(x => x.DockerImages)
            .Include(x => x.Image)
            .ThenInclude(x => x.Variables)
            .First(x => x.Id == s.Id);

        var configuration = server.ToServerConfiguration();
        
        using var client = server.Node.CreateHttpClient();

        await client.SendHandled<NodeException>(HttpMethod.Post, "servers", configuration);
    }
}