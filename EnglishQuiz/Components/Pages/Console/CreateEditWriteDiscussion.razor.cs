using EnglishQuiz.Models.Writing;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditWriteDiscussion
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private WriteDiscussion? model;

    protected override async Task OnParametersSetAsync()
    {
        model = IsEdit ? await service.GetAsync<WriteDiscussion>(Id) : new();
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
                navigation.NavigateTo($"/console/write-discussion/{model.Id}");
            else
                navigation.NavigateTo($"/console/write-discussion/");
        }
    }
}