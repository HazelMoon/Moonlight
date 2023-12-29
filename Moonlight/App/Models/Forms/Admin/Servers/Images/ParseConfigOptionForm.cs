using System.ComponentModel.DataAnnotations;

namespace Moonlight.App.Models.Forms.Admin.Servers.Images;

public class ParseConfigOptionForm
{
    [Required(ErrorMessage = "You need to specify the key of an option")]
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}