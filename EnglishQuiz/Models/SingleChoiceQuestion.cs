namespace EnglishQuiz.Models;

public class SingleChoiceQuestion
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string[] Answers { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];
}

public class SingleChoiceQuestionAnswer
{
    public int Id { get; set; } = 0;
    public string Answer { get; set; } = string.Empty;
}