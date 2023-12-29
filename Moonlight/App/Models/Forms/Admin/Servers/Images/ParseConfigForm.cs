using System.ComponentModel.DataAnnotations;

namespace Moonlight.App.Models.Forms.Admin.Servers.Images;

public class ParseConfigForm
{
    [Required(ErrorMessage = "You need to specify a type in a parse configuration")]
    public string Type { get; set; } = "";
    
    [Required(ErrorMessage = "You need to specify a file in a parse configuration")]
    public string File { get; set; } = "";
    
    [Required(ErrorMessage = "You need to specify a key in a parse configuration")]
    public string Key { get; set; } = "";
    
    [Required(ErrorMessage = "You need to specify a variable in a parse configuration")]
    public string Variable { get; set; } = "";
}