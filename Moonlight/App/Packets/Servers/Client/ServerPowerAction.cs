using Moonlight.App.Models.Enums;

namespace Moonlight.App.Packets.Servers.Client;

public class ServerPowerAction
{
    public int Id { get; set; }
    public PowerAction Action { get; set; }
}