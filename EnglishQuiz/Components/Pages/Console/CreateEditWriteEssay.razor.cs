using EnglishQuiz.Models.Writing;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditWriteEssay
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private WriteEssay? model;

    protected override async Task OnParametersSetAsync()
    {
        model = IsEdit ? await service.GetAsync<WriteEssay>(Id) : new();
    }

    private async Task SaveAsync(bool saveContinue = false)
    {
        if (model != null)
        {
            if (model.Id > 0)
                await service.EditAsync(model);
            else
                model.Id = await service.CreateAsync(model);

            if (saveContinue)
                navigation.NavigateTo($"/console/write-essay/{model.Id}");
            else
                navigation.NavigateTo($"/console/write-essay/");
        }
    }
}