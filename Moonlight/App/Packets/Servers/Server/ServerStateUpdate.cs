using Moonlight.App.Models.Enums;

namespace Moonlight.App.Packets.Servers.Server;

public class ServerStateUpdate
{
    public int Id { get; set; }
    public ServerState State { get; set; }
}