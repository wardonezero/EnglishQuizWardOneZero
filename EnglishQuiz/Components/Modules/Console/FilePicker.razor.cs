using EnglishQuiz.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace EnglishQuiz.Components.Modules.Console;

public partial class FilePicker
{
    [Parameter] public EventCallback<string> OnFileSelected { get; set; }

    private List<FileItem> Items { get; set; } = [];
    private string CurrentSubPath { get; set; } = "";
    private List<FileItem> SelectedItems { get; set; } = [];
    private bool HasSelection => SelectedItems.Any();

    private bool isCreateFolderVisible;
    private string newFolderName = "";

    private List<(string Name, string Path)> PathSegments =>
        CurrentSubPath.Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(new List<(string Name, string Path)>(), (list, segment) =>
            {
                string path = list.Any() ? $"{list.Last().Path}/{segment}" : segment;
                list.Add((segment, path));
                return list;
            });

    protected override async Task OnInitializedAsync()
    {
        await LoadFiles();
    }

    private async Task LoadFiles()
    {
        Items = await fileManager.GetFilesAsync(CurrentSubPath);
        SelectedItems.Clear();
    }

    private async Task NavigateTo(string path)
    {
        CurrentSubPath = path;
        await LoadFiles();
    }

    private async Task NavigateUp()
    {
        string[] parts = CurrentSubPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > 0)
        {
            CurrentSubPath = string.Join('/', parts.SkipLast(1));
            await LoadFiles();
        }
    }

    private void HandleItemClick(FileItem item, MouseEventArgs e)
    {
        if (e.CtrlKey)
        {
            if (SelectedItems.Contains(item))
                SelectedItems.Remove(item);
            else
                SelectedItems.Add(item);
        }
        else
        {
            SelectedItems.Clear();
            SelectedItems.Add(item);
        }
    }

    private async Task HandleDoubleClick(FileItem item)
    {
        if (item.IsFolder)
        {
            await NavigateTo(item.FullPath);
        }
        else
        {
            await OnFileSelected.InvokeAsync(item.FullPath);
        }
    }

    private bool IsSelected(FileItem item) => SelectedItems.Contains(item);

    private async Task OnUploadFiles(InputFileChangeEventArgs e)
    {
        await fileManager.UploadFilesAsync(CurrentSubPath, [.. e.GetMultipleFiles()]);
        await LoadFiles();
    }

    private async Task OnUploadFolder(InputFileChangeEventArgs e)
    {
        await fileManager.UploadFolderAsync(CurrentSubPath, [.. e.GetMultipleFiles()]);
        await LoadFiles();
    }

    private void ShowCreateFolderModal()
    {
        newFolderName = "";
        isCreateFolderVisible = true;
    }

    private async Task CreateFolder()
    {
        if (!string.IsNullOrWhiteSpace(newFolderName))
        {
            await fileManager.CreateFolderAsync(CurrentSubPath, newFolderName);
            isCreateFolderVisible = false;
            await LoadFiles();
        }
    }

    private async Task DeleteSelected()
    {
        IEnumerable<string> filesToDelete = SelectedItems.Where(i => !i.IsFolder).Select(i => i.FullPath);
        IEnumerable<FileItem> foldersToDelete = SelectedItems.Where(i => i.IsFolder);

        if (filesToDelete.Any())
            await fileManager.DeleteFilesAsync(filesToDelete);

        foreach (FileItem? folder in foldersToDelete)
            await fileManager.DeleteFolderAsync(folder.FullPath);

        await LoadFiles();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Delete" && HasSelection)
        {
            await DeleteSelected();
        }
    }

    private string GetFileIcon(string fileName)
    {
        string ext = Path.GetExtension(fileName).ToLower();
        return ext switch
        {
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".svg" => "fas fa-file-image",
            ".mp3" or ".wav" or ".ogg" or ".aac" or ".m4a" => "fas fa-file-audio",
            ".mp4" or ".mov" or ".avi" or ".mkv" or ".webm" => "fas fa-file-video",
            _ => "fas fa-file"
        };
    }
}