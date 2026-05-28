namespace EnglishQuiz.Models.Speaking;

public class Conversation : BaseQuiz
{
    public string ImageUrl { get; set; } = string.Empty;
    public string[] Texts { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] AudioUrls { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];
}