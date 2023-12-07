using System.ComponentModel.DataAnnotations;

namespace Moonlight.App.Models.Forms.Admin.Servers.Images;

public class DockerImageForm
{
    [Required(ErrorMessage = "You need to specify the name of the docker image")]
    public string Name { get; set; } = "";
    public bool IsDefault { get; set; } = false;
}