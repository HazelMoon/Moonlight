namespace Moonlight.App.Database.Entities.Servers;

public class ServerImage
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public int AllocationsNeeded { get; set; } = 1;
    public string StartupCommand { get; set; } = "";
    public string StopCommand { get; set; } = "";
    public string OnlineDetection { get; set; } = ""; // Will be regex

    public string InstallDockerImage { get; set; } = "";
    public string InstallScript { get; set; } = "";
    public string InstallShell { get; set; } = "";

    public List<ServerImageVariable> Variables { get; set; } = new();
    public List<ServerDockerImage> DockerImages { get; set; } = new();
    public string ParseConfigurations { get; set; } = "[]";
    
    public string Author { get; set; } = "";
    public string? DonateUrl { get; set; }
    public string? UpdateUrl { get; set; }
}