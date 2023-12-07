namespace Moonlight.App.Database.Entities.Servers;

public class ServerNode
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Fqdn { get; set; } = "";
    public string Token { get; set; } = "";

    public int HttpPort { get; set; } = 8080;
    public bool UseSsl { get; set; } = false;

    public List<ServerAllocation> Allocations { get; set; } = new();
}