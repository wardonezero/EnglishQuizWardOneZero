namespace EnglishQuiz.Models.Speaking;

public class Conversation : BaseQuiz
{
    public string[] Texts { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] ImageUrls { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] AudioUrls { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];
}