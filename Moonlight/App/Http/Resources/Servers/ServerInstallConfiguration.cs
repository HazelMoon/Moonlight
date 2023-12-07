namespace Moonlight.App.Http.Resources.Servers;

public class ServerInstallConfiguration
{
    public string DockerImage { get; set; } = "";
    public string Shell { get; set; } = "";
    public string Script { get; set; } = "";
}