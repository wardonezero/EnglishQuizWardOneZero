using EnglishQuiz.Models;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditReading
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private Reading? model;
    private readonly bool showImagePicker;

    protected override async Task OnParametersSetAsync()
    {
        model = IsEdit ? await service.GetAsync<Reading>(Id) : new();
    }

    private void OnSinglChoiceQuestionSaved(int questionId)
    {
        if (model != null)
        {
            if (model.SinglChoiceQuestionsIds.Contains(questionId))
                return;

            for (int i = 0; i < model.SinglChoiceQuestionsIds.Length; i++)
            {
                if (model.SinglChoiceQuestionsIds[i] == 0)
                {
                    model.SinglChoiceQuestionsIds[i] = questionId;
                    break;
                }
            }
        }
    }

    private void RemoveQuestion(int id)
    {
        if (model != null)
        {
            for (int i = 0; i < model.SinglChoiceQuestionsIds.Length; i++)
            {
                if (model.SinglChoiceQuestionsIds[i] == id)
                {
                    model.SinglChoiceQuestionsIds[i] = 0;
                    break;
                }
            }
        }
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
                navigation.NavigateTo($"/console/reading/{model.Id}");
            else
                navigation.NavigateTo($"/console/reading/");
        }
    }
}