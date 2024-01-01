using Moonlight.App.Models.Abstractions.FileAccess;

namespace Moonlight.App.Helpers.FileAccesses;

public class LocalFileAccess : IFileAccess
{
    private string currentDirectory;

    public LocalFileAccess(string initialDirectory)
    {
        currentDirectory = initialDirectory;
    }

    public async Task<FileEntry[]> List()
    {
        var entries = Directory
            .GetFileSystemEntries(currentDirectory)
            .Select(entry => GetFileEntry(entry))
            .ToArray();

        return await Task.FromResult(entries);
    }

    public async Task ChangeDirectory(string relativePath)
    {
        var newPath = Path.Combine(currentDirectory, relativePath);
        newPath = Path.GetFullPath(newPath);

        if (Directory.Exists(newPath))
        {
            currentDirectory = newPath;
        }
        else
        {
            throw new DirectoryNotFoundException($"Directory not found: {relativePath}");
        }
    }

    public async Task SetDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            currentDirectory = path;
        }
        else
        {
            throw new DirectoryNotFoundException($"Directory not found: {path}");
        }
    }

    public async Task<string> GetCurrentDirectory()
    {
        return await Task.FromResult(currentDirectory);
    }

    public async Task Delete(string path)
    {
        var fullPath = Path.Combine(currentDirectory, path);
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
        var sourcePath = Path.Combine(currentDirectory, from);
        var destinationPath = Path.Combine(currentDirectory, to);

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
        var newDirectoryPath = Path.Combine(currentDirectory, name);
        Directory.CreateDirectory(newDirectoryPath);
    }

    public async Task CreateFile(string name)
    {
        var newFilePath = Path.Combine(currentDirectory, name);
        File.Create(newFilePath).Close(); // Close the file stream to release resources
    }

    public async Task<string> ReadFile(string name)
    {
        var filePath = Path.Combine(currentDirectory, name);
        return await File.ReadAllTextAsync(filePath);
    }

    public async Task WriteFile(string name, string content)
    {
        var filePath = Path.Combine(currentDirectory, name);
        await File.WriteAllTextAsync(filePath, content);
    }

    public async Task<Stream> ReadFileStream(string name)
    {
        var filePath = Path.Combine(currentDirectory, name);
        return await Task.FromResult<FileStream>(File.OpenRead(filePath));
    }

    public async Task WriteFileStream(string name, Stream dataStream)
    {
        var filePath = Path.Combine(currentDirectory, name);
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
}
