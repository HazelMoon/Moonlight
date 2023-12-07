namespace Moonlight.App.Database.Entities.Servers;

public class ServerDockerImage
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsDefault { get; set; } = false;
}