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

public class SingleChoiceQuestionStudentAnswer
{
    public int Id { get; set; } = 0;
    public int StudentId { get; set; } = 0;
    public string Answer { get; set; } = string.Empty;
    public bool IsOmitted => string.IsNullOrWhiteSpace(Answer);
    public string Question { get; set; } = string.Empty;
    public string[] Options { get; set; } = [];
    public string CorrectAnswer { get; set; } = string.Empty;
    public bool IsCorrect => !string.IsNullOrWhiteSpace(CorrectAnswer) && !string.IsNullOrWhiteSpace(Answer) && Answer == CorrectAnswer;
}