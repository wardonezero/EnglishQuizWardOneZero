namespace EnglishQuiz.Models.Speaking;

public class ListenRepeat : BaseQuiz
{
    public string[] Sentences { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] AudioUrls { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
}