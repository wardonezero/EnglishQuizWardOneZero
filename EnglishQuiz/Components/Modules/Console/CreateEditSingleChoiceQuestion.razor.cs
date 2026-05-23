using EnglishQuiz.Models;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Modules.Console;

public partial class CreateEditSingleChoiceQuestion
{
    [Parameter] public int Id { get; set; }
    [Parameter] public EventCallback<int> OnSaved { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    private bool IsEdit => Id > 0;
    private SingleChoiceQuestion? model;
    private SingleChoiceQuestionAnswer? answer;

    protected override async Task OnParametersSetAsync()
    {
        if (IsEdit)
        {
            model = await service.GetAsync<SingleChoiceQuestion>(Id);
            answer = await service.GetAsync<SingleChoiceQuestionAnswer>(Id);
        }
        model ??= new();
        answer ??= new();
    }

    private async Task SaveAsync()
    {
        if (model != null && answer != null)
        {
            if (IsEdit)
            {
                await service.EditAsync(model);
                await service.EditAsync(answer);
                await OnSaved.InvokeAsync(model.Id);
            }
            else
            {
                int newId = await service.CreateAsync(model);
                answer.Id = newId;
                await service.CreateAsync(answer);
                await OnSaved.InvokeAsync(newId);
                model = new();
                answer = new();
            }
        }
    }
}