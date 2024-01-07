using Moonlight.App.Models.Abstractions.FileAccess;

namespace Moonlight.App.Helpers.FileAccesses;

public class DummyFileAccess : IFileAccess
{
    public Task<FileEntry[]> List()
    {
        return Task.FromResult(new[]
        {
            new FileEntry()
            {
                Name = "test.txt",
                Size = 1024,
                IsDirectory = false,
                IsFile = true,
                LastModifiedAt = DateTime.UtcNow
            },
            new FileEntry()
            {
                Name = "test.txt",
                Size = 1024,
                IsDirectory = false,
                IsFile = true,
                LastModifiedAt = DateTime.UtcNow
            },
            new FileEntry()
            {
                Name = "test.txt",
                Size = 1024,
                IsDirectory = false,
                IsFile = true,
                LastModifiedAt = DateTime.UtcNow
            },
            new FileEntry()
            {
                Name = "test.txt",
                Size = 1024,
                IsDirectory = false,
                IsFile = true,
                LastModifiedAt = DateTime.UtcNow
            },
            new FileEntry()
            {
                Name = "test.txt",
                Size = 1024,
                IsDirectory = false,
                IsFile = true,
                LastModifiedAt = DateTime.UtcNow
            }
        });
    }

    public Task ChangeDirectory(string relativePath)
    {
        throw new NotImplementedException();
    }

    public Task SetDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetCurrentDirectory()
    {
        return Task.FromResult<string>("/");
    }

    public Task Delete(string path)
    {
        throw new NotImplementedException();
    }

    public Task Move(string from, string to)
    {
        throw new NotImplementedException();
    }

    public Task CreateDirectory(string name)
    {
        throw new NotImplementedException();
    }

    public Task CreateFile(string name)
    {
        throw new NotImplementedException();
    }

    public Task<string> ReadFile(string name)
    {
        throw new NotImplementedException();
    }

    public Task WriteFile(string name, string content)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> ReadFileStream(string name)
    {
        throw new NotImplementedException();
    }

    public Task WriteFileStream(string name, Stream dataStream)
    {
        throw new NotImplementedException();
    }

    public IFileAccess Clone()
    {
        return new DummyFileAccess();
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}