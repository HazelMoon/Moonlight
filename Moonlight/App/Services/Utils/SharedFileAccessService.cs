using Moonlight.App.Models.Abstractions.FileAccess;
using Moonlight.App.Models.Enums;

namespace Moonlight.App.Services.Utils;

public class SharedFileAccessService
{
    private readonly JwtService JwtService;
    private readonly List<IFileAccess> FileAccesses = new();

    public SharedFileAccessService(JwtService jwtService)
    {
        JwtService = jwtService;
    }

    public Task<int> Register(IFileAccess fileAccess)
    {
        lock (FileAccesses)
            FileAccesses.Add(fileAccess);

        return Task.FromResult(fileAccess.GetHashCode());
    }

    public Task Unregister(IFileAccess fileAccess)
    {
        lock (FileAccesses)
        {
            if (FileAccesses.Contains(fileAccess))
                FileAccesses.Remove(fileAccess);
        }
        
        return Task.CompletedTask;
    }

    public Task<IFileAccess?> Get(int id)
    {
        lock (FileAccesses)
        {
            var fileAccess = FileAccesses.FirstOrDefault(x => x.GetHashCode() == id);

            if (fileAccess == null)
                return Task.FromResult<IFileAccess?>(null);
            
            return Task.FromResult<IFileAccess?>(fileAccess.Clone());
        }
    }

    public async Task<string> GenerateUrl(IFileAccess fileAccess)
    {
        var token = await JwtService.Create(data =>
        {
            data.Add("FileAccessId", fileAccess.GetHashCode().ToString());
        }, JwtType.FileUpload, TimeSpan.FromMinutes(6));
        
        return $"/api/upload?token={token}";
    }
}