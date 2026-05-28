namespace EnglishQuiz.Models.Speaking;

public class ListenRepeat : BaseQuiz
{
    public string MainText = string.Empty;
    public string MainImageUrl = string.Empty;
    public string MainAudioUrl = string.Empty;
    public string[] Sentences { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] ImageUrls { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] AudioUrls { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
}