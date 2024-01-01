namespace Moonlight.App.Models.Abstractions.FileAccess;

public interface IFileLaunchAccess
{
    public Task<string> GetLaunchUrl();
}