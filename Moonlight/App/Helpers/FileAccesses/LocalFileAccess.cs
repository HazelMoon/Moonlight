using Moonlight.App.Models.Abstractions.FileAccess;

namespace Moonlight.App.Helpers.FileAccesses;

public class LocalFileAccess : IFileAccess
{
    private string CurrentDirectory;
    private string RootDirectory;

    public LocalFileAccess(string rootDirectory)
    {
        CurrentDirectory = "/";
        RootDirectory = rootDirectory;
    }

    public async Task<FileEntry[]> List()
    {
        var entries = Directory
            .GetFileSystemEntries(GetRealPath(CurrentDirectory))
            .Select(entry => GetFileEntry(entry))
            .ToArray();

        return await Task.FromResult(entries);
    }

    public async Task ChangeDirectory(string relativePath)
    {
        var newPath = Path.Combine(CurrentDirectory, relativePath);
        newPath = Path.GetFullPath(newPath);

        if (Directory.Exists(RootDirectory + newPath))
        {
            CurrentDirectory = newPath;
        }
        else
        {
            throw new DirectoryNotFoundException($"Directory not found: {relativePath}");
        }
    }

    public async Task SetDirectory(string path)
    {
        if (Directory.Exists(GetRealPath(path)))
        {
            CurrentDirectory = path;
        }
        else
        {
            throw new DirectoryNotFoundException($"Directory not found: {path}");
        }
    }

    public async Task<string> GetCurrentDirectory()
    {
        return await Task.FromResult(CurrentDirectory);
    }

    public async Task Delete(string path)
    {
        var fullPath = GetRealPath(Path.Combine(CurrentDirectory, path));
        if (File.Exists(fullPath) || Directory.Exists(fullPath))
        {
            FileAttributes attributes = File.GetAttributes(fullPath);
            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Directory.Delete(fullPath, true);
            }
            else
            {
                File.Delete(fullPath);
            }
        }
        else
        {
            throw new FileNotFoundException($"File or directory not found: {path}");
        }
    }

    public async Task Move(string from, string to)
    {
        var sourcePath = GetRealPath(Path.Combine(CurrentDirectory, from));
        var destinationPath = GetRealPath(Path.Combine(CurrentDirectory, to));

        if (File.Exists(sourcePath) || Directory.Exists(sourcePath))
        {
            File.Move(sourcePath, destinationPath);
        }
        else
        {
            throw new FileNotFoundException($"File or directory not found: {from}");
        }
    }

    public async Task CreateDirectory(string name)
    {
        var newDirectoryPath = GetRealPath(Path.Combine(CurrentDirectory, name));
        Directory.CreateDirectory(newDirectoryPath);
    }

    public async Task CreateFile(string name)
    {
        var newFilePath = GetRealPath(Path.Combine(CurrentDirectory, name));
        File.Create(newFilePath).Close(); // Close the file stream to release resources
    }

    public async Task<string> ReadFile(string name)
    {
        var filePath = GetRealPath(Path.Combine(CurrentDirectory, name));
        return await File.ReadAllTextAsync(filePath);
    }

    public async Task WriteFile(string name, string content)
    {
        var filePath = GetRealPath(Path.Combine(CurrentDirectory, name));
        await File.WriteAllTextAsync(filePath, content);
    }

    public async Task<Stream> ReadFileStream(string name)
    {
        var filePath = GetRealPath(Path.Combine(CurrentDirectory, name));
        return await Task.FromResult(File.OpenRead(filePath));
    }

    public async Task WriteFileStream(string name, Stream dataStream)
    {
        var memoryStream = new MemoryStream();

        await dataStream.CopyToAsync(memoryStream);
        
        await memoryStream.DisposeAsync();
        
        return;
        
        var filePath = GetRealPath(Path.Combine(CurrentDirectory, name));
        using (var fileStream = File.Create(filePath))
        {
            await dataStream.CopyToAsync(fileStream);
        }
    }


    private FileEntry GetFileEntry(string path)
    {
        var fileInfo = new FileInfo(path);
        var isFile = (File.GetAttributes(path) & FileAttributes.Directory) == 0;

        return new FileEntry
        {
            Name = Path.GetFileName(path),
            Size = isFile ? fileInfo.Length : 0,
            IsFile = isFile,
            IsDirectory = !isFile,
            LastModifiedAt = fileInfo.LastWriteTime
        };
    }

    public string GetRealPath(string? overrideCurrentDir = null)
    {
        if (string.IsNullOrEmpty(overrideCurrentDir))
            return RootDirectory + CurrentDirectory;

        return RootDirectory + overrideCurrentDir;
    }
    
    public IFileAccess Clone()
    {
        return new LocalFileAccess(RootDirectory)
        {
            CurrentDirectory = CurrentDirectory
        };
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
