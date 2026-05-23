namespace EnglishQuiz.Models;

public class Reading : BaseQuiz
{
    public string Text { get; set; } = string.Empty;
    public List<int> SinglChoiceQuestionsIds { get; set; } = [];
    public List<int> MultiChoiceQuestionsIds { get; set; } = [];
    public int QuestionsCount => SinglChoiceQuestionsIds.Count + MultiChoiceQuestionsIds.Count;
}