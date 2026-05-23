namespace EnglishQuiz.Models;

public class MultiChoiceQuestion : BaseQuiz
{
    public List<string> Answers { get; set; } = new(4);
}

public class MultiChoiceQuestionAnswer
{
    public int Id { get; set; } = 0;
    public List<string> Answers { get; set; } = new(4);
}