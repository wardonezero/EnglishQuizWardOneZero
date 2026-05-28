namespace EnglishQuiz.Models.Writing;

public class BuildSentence : BaseQuiz
{
    public string[] Prompts { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];
    public string[] Sentences { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];

    public string[] GetSentences(string[] answers)
    {
        if (answers.Length != 10)
            throw new InvalidOperationException("BuildSentence GetSentences  answers.Length must be 10");
        string[] shuffledSentences = new string[10];
        string[] words;
        for (byte i = 0; i < 10; i++)
        {
            string sentence = answers[i];
            words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] shuffledWords = [.. words.OrderBy(w => Random.Shared.Next())];
            shuffledSentences[i] = string.Join(' ', shuffledWords);
        }
        return shuffledSentences;
    }
}

public class BuildSentenceAnswer
{
    public int Id { get; set; } = 0;
    public string[] AnswerSentences { get; set; } = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,];

    public Dictionary<byte, bool> CheckAnswer(string[] studentAnswer)
    {
        if (studentAnswer.Length != 10)
            throw new InvalidOperationException("BuildSentenceAnswer CheckAnswer  studentAnswer.Length must be 10");
        Dictionary<byte, bool> results = new(10);
        for (byte i = 0; i < 10; i++)
        {
            bool isCorrect = string.Equals(studentAnswer[i], AnswerSentences[i], StringComparison.OrdinalIgnoreCase);
            results.Add(i, isCorrect);
        }
        return results;
    }
}