namespace Moonlight.App.Models.Server.Api.Resources;

public class Status
{
    public bool IsBooting { get; set; }
    public string Version { get; set; } = "N/A";
}