namespace EnglishQuiz.Models;

public class SingleChoiceQuestion : BaseQuiz
{
    public List<string> Answers { get; set; } = new(4);
}

public class SingleChoiceQuestionAnswer
{
    public int Id { get; set; } = 0;
    public string Answer { get; set; } = string.Empty;
}