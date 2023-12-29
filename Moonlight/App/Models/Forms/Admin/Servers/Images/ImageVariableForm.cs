using System.ComponentModel.DataAnnotations;

namespace Moonlight.App.Models.Forms.Admin.Servers.Images;

public class ImageVariableForm
{
    [Required(ErrorMessage = "You need to specify the key of the variable")]
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}