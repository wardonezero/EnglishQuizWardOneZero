using EnglishQuiz.Models;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditListening
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private Listening? model;
    private bool showImagePicker;

    protected override async Task OnParametersSetAsync()
    {
        model = IsEdit ? await service.GetAsync<Listening>(Id) : new();
    }

    private void OnImageSelected(string path)
    {
        if (model != null)
        {
            model.ImageUrl = path;
            showImagePicker = false;
        }
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
                navigation.NavigateTo($"/console/listening/{model.Id}");
            else
                navigation.NavigateTo($"/console/listening/");
        }
    }
}