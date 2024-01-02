using Microsoft.EntityFrameworkCore;
using Moonlight.App.Database.Entities;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Database.Entities.Store;
using Moonlight.App.Exceptions;
using Moonlight.App.Helpers;
using Moonlight.App.Models.Abstractions.Services;
using Moonlight.App.Models.Abstractions.Services.Extensions;
using Moonlight.App.Repositories;
using Moonlight.App.Services.Servers;
using Moonlight.App.Services.ServiceManage;
using Newtonsoft.Json;

namespace Moonlight.App.Actions.Servers;

public class ServerActions : ServiceActions, IServiceFtpActions, IServiceFileManagerActions
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
        
        var allocations = allocationRepo
            .Get()
            .FromSqlRaw(
                $"SELECT * FROM `ServerAllocations` WHERE ServerId IS NULL AND ServerNodeId={node.Id} LIMIT {image.AllocationsNeeded}")
            .ToArray();
        
        Logger.Debug(allocations.Length.ToString());

        if (allocations.Length < image.AllocationsNeeded)
            throw new DisplayException("Not enough free allocations on the node found");
        
        var server = new Server()
        {
            Service = service,
            Cpu = config.Cpu,
            Memory = config.Memory,
            Disk = config.Disk,
            Node = node,
            MainAllocation = allocations.First(),
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

        foreach (var allocation in allocations)
            server.Allocations.Add(allocation);

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
        var serverRepo = provider.GetRequiredService<Repository<Server>>();
        var serverVariableRepo = provider.GetRequiredService<Repository<ServerVariable>>();

        var server = serverRepo
            .Get()
            .Include(x => x.Variables)
            .Include(x => x.MainAllocation)
            .First(x => x.Service.Id == service.Id);

        var variables = server.Variables.ToArray();
        
        server.Variables.Clear();
        
        serverRepo.Update(server);

        foreach (var variable in variables)
            serverVariableRepo.Delete(variable);
        
        serverRepo.Delete(server);
        
        return Task.CompletedTask;
    }

    public async Task<bool> AuthenticateFtpLogin(IServiceProvider provider, User user, int resourceId, HttpRequest request)
    {
        var requestHelper = provider.GetRequiredService<NodeRequestHelper>();

        if (!await requestHelper.Verify(request))
            return false;

        var serverRepo = provider.GetRequiredService<Repository<Server>>();
        var server = serverRepo
            .Get()
            .Include(x => x.Service)
            .FirstOrDefault(x => x.Id == resourceId);

        if (server == null)
            return false;

        var serviceManageService = provider.GetRequiredService<ServiceManageService>();

        return await serviceManageService.CheckAccess(server.Service, user);
    }

    public Task<bool> ProcessFileUpload(IServiceProvider provider, User user, Service service, string fileName, Stream stream)
    {
        return Task.FromResult(true);
    }
}