using System.ComponentModel.DataAnnotations;

namespace Moonlight.App.Models.Forms.Admin.Servers.Images;

public class ImageForm
{
    [Required(ErrorMessage = "You need to specify a name")]
    public string Name { get; set; } = "";

    [Range(0, 20, ErrorMessage = "The allocations needed amount need to be between 0 and 20")]
    public int AllocationsNeeded { get; set; } = 1;
    [Required(ErrorMessage = "You need to specify a startup command")]
    public string StartupCommand { get; set; } = "";
    [Required(ErrorMessage = "You need to specify a stop command")]
    public string StopCommand { get; set; } = "^C";
    [Required(ErrorMessage = "You need to specify a online detection regex string")]
    public string OnlineDetection { get; set; } = "Done. "; // Will be regex

    [Required(ErrorMessage = "You need to specify a install docker image")]
    public string InstallDockerImage { get; set; } = "debian:bookworm-slim";
    [Required(ErrorMessage = "You need to provide a install script")]
    public string InstallScript { get; set; } = "#! /bin/bash";
    public string InstallShell { get; set; } = "/bin/bash";
    
    public string Author { get; set; } = "You i guess";
    public string? DonateUrl { get; set; }
    public string? UpdateUrl { get; set; }
}