namespace EnglishQuiz.Models;

public class Reading : BaseQuiz
{
    public string Text { get; set; } = string.Empty;
    public int[] SinglChoiceQuestionsIds { get; set; } = [0, 0, 0, 0, 0, 0, 0, 0, 0];
    public int[] MultiChoiceQuestionsIds { get; set; } = [0, 0, 0, 0, 0, 0, 0, 0, 0];
    public int QuestionsCount => SinglChoiceQuestionsIds.Length + MultiChoiceQuestionsIds.Length;
}