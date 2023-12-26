using Moonlight.App.Helpers;
using Moonlight.App.Models.Enums;

namespace Moonlight.App.Models.Servers;

public class ServerMeta
{
    // Events
    public SmartEventHandler OnStateChanged { get; set; } = new();
    public SmartEventHandler OnResourcesChanged { get; set; } = new();
    public SmartEventHandler<string> OnConsoleMessage { get; set; } = new();
    
    // Data
    public ServerState State { get; set; }
    public DateTime LastChangeTimestamp { get; set; }
    public List<string> ConsoleMessages { get; set; } = new();
}