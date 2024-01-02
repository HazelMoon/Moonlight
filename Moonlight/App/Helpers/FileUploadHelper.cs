using Moonlight.App.Database.Entities.Store;
using Moonlight.App.Models.Enums;
using Moonlight.App.Services.Utils;

namespace Moonlight.App.Helpers;

public class FileUploadHelper
{
    private readonly JwtService JwtService;

    public FileUploadHelper(JwtService jwtService)
    {
        JwtService = jwtService;
    }

    public async Task<string> GenerateUploadUrl(Service service, string path)
    {
        var token = await JwtService.Create(data =>
        {
            data.Add("Path", path);
            data.Add("ServiceId", service.Id.ToString());
        }, JwtType.FileUpload, TimeSpan.FromMinutes(5));

        return $"/api/upload?token={token}";
    }
}