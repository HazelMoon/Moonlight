using System.ComponentModel;

namespace Moonlight.App.Actions.Servers;

public class ServerConfig
{
    [Description("The amount of cpu cores for a server instance. 100% = 1 Core")]
    public int Cpu { get; set; } = 100;

    [Description("The amount of memory in megabytes for a server instance")]
    public int Memory { get; set; } = 1024;

    [Description("The amount of disk space in megabytes for a server instance")]
    public int Disk { get; set; } = 1024;

    [Description("The id of the image to use for a server")]
    public int ImageId { get; set; } = 1;
}