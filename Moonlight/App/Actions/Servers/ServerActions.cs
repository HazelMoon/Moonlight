using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Database.Entities.Store;
using Moonlight.App.Exceptions;
using Moonlight.App.Models.Abstractions.Services;
using Moonlight.App.Repositories;
using Moonlight.App.Services.Servers;
using Newtonsoft.Json;

namespace Moonlight.App.Actions.Servers;

public class ServerActions : ServiceActions
{
    public override async Task Create(IServiceProvider provider, Service service)
    {
        var serverRepo = provider.GetRequiredService<Repository<Server>>();
        var imageRepo = provider.GetRequiredService<Repository<ServerImage>>();
        var nodeRepo = provider.GetRequiredService<Repository<ServerNode>>();
        var allocationRepo = provider.GetRequiredService<Repository<ServerAllocation>>();
        
        var config =
            JsonConvert.DeserializeObject<ServerConfig>(service.ConfigJsonOverride ?? service.Product.ConfigJson)!;

        var image = imageRepo
            .Get()
            .Include(x => x.DockerImages)
            .Include(x => x.Variables)
            .FirstOrDefault(x => x.Id == config.ImageId);

        if (image == null)
            throw new DisplayException("An image with this is is not found");
        
        var node = nodeRepo
            .Get()
            .First();
        
        var freeAllocations = allocationRepo
            .Get()
            .FromSqlRaw(
                $"SELECT * FROM `ServerAllocations` WHERE ServerId IS NULL AND ServerNodeId={node.Id} LIMIT {image.AllocationsNeeded}")
            .ToArray();

        if (freeAllocations.Length < image.AllocationsNeeded)
            throw new DisplayException("Not enough free allocations on the node found");

        var mainAllocation = freeAllocations.First();
        var additionalAllocations = freeAllocations
            .Where(x => x.Id != mainAllocation.Id)
            .ToArray();

        var server = new Server()
        {
            Service = service,
            Cpu = config.Cpu,
            Memory = config.Memory,
            Disk = config.Disk,
            Node = node,
            MainAllocation = mainAllocation,
            AdditionalAllocations = additionalAllocations.ToList(),
            Image = image,
            OverrideStartupCommand = null,
            DockerImageIndex = image.DockerImages.IndexOf( // This selects the default docker image, if there is none it will pick the last
                image.DockerImages.FirstOrDefault(x => x.IsDefault) 
                ?? image.DockerImages.Last()),
            Variables = image.Variables.Select(x => new ServerVariable()
            {
                Key = x.Key,
                Value = x.Value
            }).ToList()
        };

        serverRepo.Add(server);

        var serverService = provider.GetRequiredService<ServerService>();
        await serverService.Sync(server);

        // Trigger reinstall here
    }

    public override Task Update(IServiceProvider provider, Service service)
    {
        return Task.CompletedTask;
    }

    public override Task Delete(IServiceProvider provider, Service service)
    {
        return Task.CompletedTask;
    }
}