using EnglishQuiz.Models.Writing;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditBuildSentence
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private BuildSentence? model;
    private BuildSentenceAnswer? answer;
    private string? errorMessage;

    protected override async Task OnParametersSetAsync()
    {
        model = IsEdit ? await service.GetAsync<BuildSentence>(Id) : new();
        answer = IsEdit ? await service.GetAsync<BuildSentenceAnswer>(Id) : new();
    }

    private async Task SaveAsync(bool saveContinue = false)
    {
        if (model != null && answer != null)
        {
            try
            {
                model.Sentences = model.GetSentences(answer.AnswerSentences);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            if (model.Id > 0 && answer.Id > 0)
            {
                await service.EditAsync(model);
                await service.EditAsync(answer);
            }
            else
            {
                model.Id = await service.CreateAsync(model);
                answer.Id = model.Id;
                await service.CreateAsync(answer);
            }

            if (saveContinue)
                navigation.NavigateTo($"/console/build-sentence/{model.Id}");
            else
                navigation.NavigateTo($"/console/build-sentence/");
        }
    }
}