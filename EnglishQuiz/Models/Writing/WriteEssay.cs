namespace EnglishQuiz.Models.Writing;

public class WriteEssay : BaseQuiz
{
    public string Situation { get; set; } = string.Empty;
    public string Instruction { get; set; } = string.Empty;
    public string[] Instructions { get; set; } = [string.Empty, string.Empty, string.Empty];
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
}