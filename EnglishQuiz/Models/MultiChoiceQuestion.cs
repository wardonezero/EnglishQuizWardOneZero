namespace EnglishQuiz.Models;

public class MultiChoiceQuestion
{
    public int Id { get; set; }

    public string Question { get; set; } = string.Empty;

    public string[] Answers { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];

}

public class MultiChoiceQuestionAnswer
{
    public int Id { get; set; } = 0;
    public string[] Answers { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty,];

}