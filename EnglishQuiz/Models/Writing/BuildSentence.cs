namespace EnglishQuiz.Models.Writing;

public class BuildSentence : BaseQuiz
{
    public string[] Prompts { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] Sentences { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
}

public class BuildSentenceAnswer
{
    public int Id { get; set; } = 0;
    public string[] AnswerSentences { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
}