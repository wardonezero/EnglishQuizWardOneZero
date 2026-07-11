using Microsoft.AspNetCore.Components;
using System.Timers;

namespace EnglishQuiz.Components.Modules.QuizViews;

public sealed partial class QuizNavigation : IDisposable
{
    [Parameter] public string Section { get; set; } = string.Empty;
    [Parameter] public byte QuestionCount { get; set; } = 0;
    [Parameter] public byte CurrentQuestion { get; set; } = 1;

    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnBack { get; set; }

    private byte DisplayQuestion => (byte)Math.Clamp(CurrentQuestion, 1, Math.Max(1, (int)QuestionCount));

    private bool showTimer = true;
    private System.Timers.Timer? timer;
    private TimeSpan elapsed = TimeSpan.Zero;
    private string TimerDisplay => $"{elapsed:mm\\:ss}";

    protected override async Task OnInitializedAsync()
    {
        if (showTimer)
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();
        }
        await base.OnInitializedAsync();
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        elapsed = elapsed.Add(TimeSpan.FromSeconds(1));
        _ = InvokeAsync(StateHasChanged);
    }

    private async Task OnNextClicked()
    {
        if (QuestionCount == 0) return;
        await OnNext.InvokeAsync(null);
    }

    private async Task OnBackClicked()
    {
        if (QuestionCount == 0) return;
        if (DisplayQuestion > 1)
            await OnBack.InvokeAsync(null);
    }

    public void Dispose()
    {
        if (timer != null)
        {
            timer.Stop();
            timer.Elapsed -= TimerElapsed;
            timer.Dispose();
            timer = null;
        }
        GC.SuppressFinalize(this);
    }
}