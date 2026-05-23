using EnglishQuiz.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EnglishQuiz.Services;

public sealed class FileManagerService(IWebHostEnvironment env)
{
    private readonly string _dataPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, "..", "Data"));
    private const int oneGB = 1_048_576;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg",
        ".mp3", ".wav", ".ogg", ".aac", ".m4a",
        ".mp4", ".mov", ".avi", ".mkv", ".webm"
    };

    public async Task<List<FileItem>> GetFilesAsync(string? subPath = null)
    {
        string targetPath = ResolvePath(subPath);

        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);

        List<FileItem> items = [];

        foreach (string dir in Directory.GetDirectories(targetPath))
        {
            items.Add(CreateFileItem(dir, true));
        }

        foreach (string file in Directory.GetFiles(targetPath))
        {
            if (AllowedExtensions.Contains(Path.GetExtension(file)))
            {
                items.Add(CreateFileItem(file, false));
            }
        }

        return await Task.FromResult(items);
    }

    public Task RenameFileAsync(string oldPath, string newName)
    {
        string oldFullPath = ResolvePath(oldPath);

        if (!Directory.Exists(oldFullPath) && !IsFileAllowed(oldFullPath))
            throw new UnauthorizedAccessException("This file type is not allowed.");

        if (!Directory.Exists(oldFullPath) && !AllowedExtensions.Contains(Path.GetExtension(newName)))
            throw new UnauthorizedAccessException("New file extension is not allowed.");

        string directory = Path.GetDirectoryName(oldFullPath)!;
        string newFullPath = Path.Combine(directory, newName);

        if (File.Exists(oldFullPath))
        {
            File.Move(oldFullPath, newFullPath);
        }

        return Task.CompletedTask;
    }

    public async Task UploadFilesAsync(string subPath, IReadOnlyList<IBrowserFile> files)
    {
        string targetDir = ResolvePath(subPath);

        if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);

        foreach (var file in files)
        {
            if (!AllowedExtensions.Contains(Path.GetExtension(file.Name)))
                continue;

            string filePath = Path.Combine(targetDir, file.Name);
            using var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 50); // 50MB limit
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }
    }

    public async Task UploadFolderAsync(string subPath, IReadOnlyList<IBrowserFile> files)
    {
        // IBrowserFile for folder upload provides the relative path in the 'Name' or we might need to parse it if provided as a flat list with paths.
        // Blazor's <InputFile> with 'webkitdirectory' provides files with relative paths.
        foreach (var file in files)
        {
            // Note: In Blazor, file.Name usually contains the relative path if webkitdirectory is used, 
            // but sometimes it's just the name and we need to handle the structure.
            // If the browser provides relative paths, we need to ensure the subdirectories are created.

            if (!AllowedExtensions.Contains(Path.GetExtension(file.Name)))
                continue;

            string targetFilePath = ResolvePath(Path.Combine(subPath, file.Name));
            string? targetFileDir = Path.GetDirectoryName(targetFilePath);

            if (!string.IsNullOrEmpty(targetFileDir) && !Directory.Exists(targetFileDir))
                Directory.CreateDirectory(targetFileDir);

            using var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 50);
            using var fileStream = new FileStream(targetFilePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }
    }


    public Task DeleteFilesAsync(IEnumerable<string> paths)
    {
        foreach (string path in paths)
        {
            string fullPath = ResolvePath(path);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        return Task.CompletedTask;
    }

    public Task RenameFolderAsync(string oldPath, string newName)
    {
        string oldFullPath = ResolvePath(oldPath);
        string parentDir = Path.GetDirectoryName(oldFullPath)!;
        string newFullPath = Path.Combine(parentDir, newName);

        if (Directory.Exists(oldFullPath))
        {
            Directory.Move(oldFullPath, newFullPath);
        }

        return Task.CompletedTask;
    }

    public Task CreateFolderAsync(string subPath, string folderName)
    {
        string targetDir = ResolvePath(Path.Combine(subPath ?? string.Empty, folderName));

        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

        return Task.CompletedTask;
    }

    public Task DeleteFolderAsync(string path)
    {
        string fullPath = ResolvePath(path);

        if (fullPath == _dataPath)
            throw new UnauthorizedAccessException("Cannot delete the Data root directory.");

        if (Directory.Exists(fullPath))
        {
            Directory.Delete(fullPath, true);
        }

        return Task.CompletedTask;
    }

    private bool IsFileAllowed(string path)
    {
        return AllowedExtensions.Contains(Path.GetExtension(path));
    }

    private string ResolvePath(string? subPath)
    {
        string combined = string.IsNullOrWhiteSpace(subPath)
            ? _dataPath
            : Path.GetFullPath(Path.Combine(_dataPath, subPath));

        return !combined.StartsWith(_dataPath, StringComparison.OrdinalIgnoreCase)
            ? throw new UnauthorizedAccessException("Path access denied: outside of Data directory.")
            : combined;
    }

    private FileItem CreateFileItem(string fullPath, bool isFolder)
    {
        return new FileItem(
            Path.GetFileName(fullPath),
            Path.GetRelativePath(_dataPath, fullPath),
            isFolder
        );
    }
}