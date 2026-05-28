using EnglishQuiz.Models;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditListening
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private Listening? model;
    private bool showImagePicker;
    private string? errorMessage;
    private string[] availableVoices = [];
    private string? selectedVoice;
    private string originalText = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        availableVoices = configuration.GetSection("SpeechConfiguration:Voices").Get<string[]>() ?? [];
        model = IsEdit ? await service.GetAsync<Listening>(Id) : new();
        originalText = model?.Text ?? string.Empty;
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
            if (model.Id <= 0)
                model.Id = await service.CreateAsync(model);
            try
            {
                bool isNew;
                bool isChanged;
                if (!string.IsNullOrWhiteSpace(model.Text))
                {
                    isNew = string.IsNullOrEmpty(model.AudioUrl);
                    isChanged = model.Text != originalText;
                    if (isNew || isChanged)
                    {
                        model.AudioUrl = await audio.GenerateAudioAsync(model.Id, $"main", "speaking", "listen-repeat", model.Text, selectedVoice);
                        originalText = model.Text;
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            await service.EditAsync(model);

            if (saveContinue)
                navigation.NavigateTo($"/console/listening/{model.Id}");
            else
                navigation.NavigateTo($"/console/listening/");
        }
    }
}