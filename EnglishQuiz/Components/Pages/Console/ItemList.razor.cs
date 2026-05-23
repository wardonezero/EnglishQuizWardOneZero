using EnglishQuiz.Models;
using EnglishQuiz.Models.Interfaces;
using EnglishQuiz.Models.Speaking;
using EnglishQuiz.Models.Writing;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class ItemList
{
    [Parameter] public string ListName { get; set; } = string.Empty;
    private List<IListItem>? model;
    private readonly int page = 1;

    protected override async Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        model = ListName switch
        {
            "listening" => [.. (await service.GetPagedAsync<Listening>(page)).Cast<IListItem>()],
            "listen-repeat" => [.. (await service.GetPagedAsync<ListenRepeat>(page)).Cast<IListItem>()],
            "conversations" => [.. (await service.GetPagedAsync<Conversation>(page)).Cast<IListItem>()],
            "reading" => [.. (await service.GetPagedAsync<Reading>(page)).Cast<IListItem>()],
            "build-sentence" => [.. (await service.GetPagedAsync<BuildSentence>(page)).Cast<IListItem>()],
            "write-essay" => [.. (await service.GetPagedAsync<WriteEssay>(page)).Cast<IListItem>()],
            "write-discussion" => [.. (await service.GetPagedAsync<WriteDiscussion>(page)).Cast<IListItem>()],
            "quizzes" => [.. (await service.GetPagedAsync<Quiz>(page)).Cast<IListItem>()],
            _ => []
        };
    }

    private void CreateNewAsync()
    {
        navigation.NavigateTo($"/console/{ListName}/create");
    }
    private void EditAsync(int id)
    {
        navigation.NavigateTo($"/console/{ListName}/{id}");
    }
}