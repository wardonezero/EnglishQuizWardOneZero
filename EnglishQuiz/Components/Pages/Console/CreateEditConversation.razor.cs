using EnglishQuiz.Models.Speaking;
using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Console;

public partial class CreateEditConversation
{
    [Parameter] public int Id { get; set; }
    private bool IsEdit => Id > 0;
    private Conversation? model;
    private bool showImagePicker;
    private string? errorMessage;
    private string[] availableVoices = [];
    private string? selectedVoice;

    private string[] originalTexts = new string[7];
    private readonly string originalMainText = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        availableVoices = configuration.GetSection("SpeechConfiguration:Voices").Get<string[]>() ?? [];
        model = IsEdit ? await service.GetAsync<Conversation>(Id) : new();
        originalTexts = model?.Texts ?? [];
    }
    private void OnImageSelected(string path)
    {
        if (model != null)
        {
            model.ImageUrl = path;
            showImagePicker = false;
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
                for (byte i = 0; i < 4; i++)
                {
                    if (string.IsNullOrWhiteSpace(model.Texts[i])) continue;
                    isNew = string.IsNullOrWhiteSpace(model.AudioUrls[i]);
                    isChanged = model.Texts[i] != originalTexts[i];
                    if (!isNew && !isChanged) continue;
                    model.AudioUrls[i] = await audio.GenerateAudioAsync(model.Id, $"sentence_{i}", "speaking", "conversation", model.Texts[i], selectedVoice);
                    originalTexts[i] = model.Texts[i];
                }
                await service.EditAsync(model);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            if (saveContinue)
                navigation.NavigateTo($"/console/conversation/{model.Id}");
            else
                navigation.NavigateTo($"/console/conversation/");
        }
    }
}