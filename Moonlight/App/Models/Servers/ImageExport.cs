namespace Moonlight.App.Models.Server;

public class ImageExport
{
    public string Name { get; set; } = "";

    public int AllocationsNeeded { get; set; } = 1;
    public string StartupCommand { get; set; } = "";
    public string StopCommand { get; set; } = "";
    public string OnlineDetection { get; set; } = ""; // Will be regex

    public string InstallDockerImage { get; set; } = "";
    public string InstallScript { get; set; } = "";
    public string InstallShell { get; set; } = "";

    // We use the "Image" prefix for the variables to stop mappy from trying to map it
    public List<Variable> ImageVariables { get; set; } = new();
    public List<DockerImage> ImageDockerImages { get; set; } = new();
    
    public string Author { get; set; } = "";
    public string? DonateUrl { get; set; }
    public string? UpdateUrl { get; set; }
    
    public class Variable
    {
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
    }
    
    public class DockerImage
    {
        public string Name { get; set; } = "";
        public bool IsDefault { get; set; } = false;
    }
}