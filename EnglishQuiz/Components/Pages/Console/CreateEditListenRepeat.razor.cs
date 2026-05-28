using EnglishQuiz.Models.Speaking;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditListenRepeat
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private ListenRepeat? model;
    private readonly bool[] showImagePickers = [false, false, false, false, false, false, false];
    private string? errorMessage;
    private string[] availableVoices = [];
    private string? selectedVoice;

    private string[] originalTexts = new string[7];
    private readonly string originalMainText = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        availableVoices = configuration.GetSection("SpeechConfiguration:Voices").Get<string[]>() ?? [];
        model = IsEdit ? await service.GetAsync<ListenRepeat>(Id) : new();
        originalTexts = model?.Sentences ?? [];
    }
    private void OnImageSelected(byte index, string path)
    {
        if (model != null)
        {
            model.ImageUrls[index] = path;
            showImagePickers[index] = false;
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
                for (byte i = 0; i < 7; i++)
                {
                    if (string.IsNullOrWhiteSpace(model.Sentences[i])) continue;
                    isNew = string.IsNullOrWhiteSpace(model.AudioUrls[i]);
                    isChanged = model.Sentences[i] != originalTexts[i];
                    if (!isNew && !isChanged) continue;
                    model.AudioUrls[i] = await audio.GenerateAudioAsync(model.Id, $"sentence_{i}", "speaking", "listen-repeat", model.Sentences[i], selectedVoice);
                    originalTexts[i] = model.Sentences[i];
                }
                await service.EditAsync(model);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            if (saveContinue)
                navigation.NavigateTo($"/console/listen-repeat/{model.Id}");
            else
                navigation.NavigateTo($"/console/listen-repeat/");
        }
    }
}