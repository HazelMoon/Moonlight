using System.ComponentModel.DataAnnotations;

namespace Moonlight.App.Models.Forms.Admin.Servers.Nodes;

public class NodeForm
{
    [Required(ErrorMessage = "You need to provide an name")]
    public string Name { get; set; } = "";
    
    [Required(ErrorMessage = "You need to provide a fqdn")]
    public string Fqdn { get; set; } = "";
    
    public int HttpPort { get; set; } = 8080;
    public bool UseSsl { get; set; } = false;
}