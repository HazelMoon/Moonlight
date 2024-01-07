using System.Net;
using System.Text;
using FluentFTP;
using Moonlight.App.Models.Abstractions.FileAccess;

namespace Moonlight.App.Helpers.FileAccesses;

public class FtpFileAccess : IFileAccess
{
    private readonly FtpClient Client;
    private string CurrentDirectory = "/";

    private string Host;
    private int Port;
    private string Username;
    private string Password;

    public FtpFileAccess(string host, int port, string username, string password)
    {
        Host = host;
        Port = port;
        Username = username;
        Password = password;
        
        Client = new FtpClient();
        Client.Host = Host;
        Client.Port = Port;
        Client.Credentials = new NetworkCredential(Username, Password);
        Client.Config.DataConnectionType = FtpDataConnectionType.PASV;
    }

    public async Task<FileEntry[]> List()
    {
        await EnsureConnected();

        var items = Client.GetListing() ?? Array.Empty<FtpListItem>();
        return items.Select(item => new FileEntry
        {
            Name = item.Name,
            IsDirectory = item.Type == FtpObjectType.Directory,
            IsFile = item.Type == FtpObjectType.File,
            LastModifiedAt = item.Modified,
            Size = item.Size
        }).ToArray();
    }

    public async Task ChangeDirectory(string relativePath)
    {
        await EnsureConnected();

        var newPath = Path.Combine(CurrentDirectory, relativePath);
        newPath = Path.GetFullPath(newPath);

        Client.SetWorkingDirectory(newPath);
        CurrentDirectory = Client.GetWorkingDirectory();
    }

    public async Task SetDirectory(string path)
    {
        await EnsureConnected();

        Client.SetWorkingDirectory(path);
    }

    public Task<string> GetCurrentDirectory()
    {
        return Task.FromResult(CurrentDirectory);
    }

    public async Task Delete(string path)
    {
        await EnsureConnected();

        if (Client.FileExists(path))
            Client.DeleteFile(path);
        else
            Client.DeleteDirectory(path);
    }

    public async Task Move(string from, string to)
    {
        await EnsureConnected();

        Client.Rename(from, to);
    }

    public async Task CreateDirectory(string name)
    {
        await EnsureConnected();

        Client.CreateDirectory(name);
    }

    public async Task CreateFile(string name)
    {
        await EnsureConnected();

        using var stream = new MemoryStream();
        Client.UploadStream(stream, name);
    }

    public async Task<string> ReadFile(string name)
    {
        await EnsureConnected();

        await using var stream = Client.OpenRead(name);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    public async Task WriteFile(string name, string content)
    {
        await EnsureConnected();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        Client.UploadStream(stream, name);
    }

    public async Task<Stream> ReadFileStream(string name)
    {
        await EnsureConnected();

        return Client.OpenRead(name);
    }

    public async Task WriteFileStream(string name, Stream dataStream)
    {
        await EnsureConnected();
        
        Client.UploadStream(dataStream, name, FtpRemoteExists.Overwrite);
    }


    private Task EnsureConnected()
    {
        if (!Client.IsConnected)
        {
            Client.Connect();
            
            // This will set the correct current directory
            // on cloned or reconnected FtpFileAccess instances
            if(CurrentDirectory != "/")
                Client.SetWorkingDirectory(CurrentDirectory);
        }

        return Task.CompletedTask;
    }
    
    public IFileAccess Clone()
    {
        return new FtpFileAccess(Host, Port, Username, Password)
        {
            CurrentDirectory = CurrentDirectory
        };
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}
