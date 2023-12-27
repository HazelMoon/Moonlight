using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Http.Resources.Servers;

namespace Moonlight.App.Extensions;

public static class ServerExtensions
{
    public static ServerConfiguration ToServerConfiguration(this Server server)
    {
        var config = new ServerConfiguration();

        config.Id = server.Id;
        config.StartupCommand = server.OverrideStartupCommand ?? server.Image.StartupCommand;

        ServerDockerImage dockerImage;

        // This prevents a invalid docker image index from breaking things
        if (server.Image.DockerImages.Count > server.DockerImageIndex)
            dockerImage = server.Image.DockerImages[server.DockerImageIndex];
        else
            dockerImage = server.Image.DockerImages.Last();
        
        config.Image = new()
        {
            DockerImage = dockerImage.Name,
            StopCommand = server.Image.StopCommand,
            OnlineDetection = server.Image.OnlineDetection
        };

        config.Allocations = server.Allocations.Select(x => new ServerConfiguration.AllocationData()
        {
            Port = x.Port
        }).ToList();

        config.Limits = new()
        {
            Cpu = server.Cpu,
            Memory = server.Memory,
            Disk = server.Disk,
            EnableOomKill = false,
            DisableSwap = false,
            PidsLimit = 100
        };

        config.MainAllocation = new()
        {
            Port = server.MainAllocation.Port
        };

        config.Variables = server.Variables
            .ToDictionary(x => x.Key, x => x.Value);
        
        return config;
    }

    public static ServerInstallConfiguration ToServerInstallConfiguration(this Server server)
    {
        var config = new ServerInstallConfiguration()
        {
            DockerImage = server.Image.InstallDockerImage,
            Shell = server.Image.InstallShell,
            Script = server.Image.InstallScript
        };

        return config;
    }
}