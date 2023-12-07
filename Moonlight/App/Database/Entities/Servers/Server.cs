using Moonlight.App.Database.Entities.Store;

namespace Moonlight.App.Database.Entities.Servers;

public class Server
{
    public int Id { get; set; }
    public Service Service { get; set; }
    
    public int Cpu { get; set; }
    public int Memory { get; set; }
    public int Disk { get; set; }
    
    public ServerImage Image { get; set; }
    public int DockerImageIndex { get; set; } = 0;
    public string? OverrideStartupCommand { get; set; }
    public List<ServerVariable> Variables { get; set; } = new();
    
    public ServerNode Node { get; set; }
    public ServerAllocation MainAllocation { get; set; }
    public List<ServerAllocation> AdditionalAllocations { get; set; }
}