namespace EnglishQuiz.Models;

public class Quiz : BaseQuiz
{
    public int ListeningId { get; set; }
    public int ListeningEasyId { get; set; }
    public int ListeningHardId { get; set; }
    public int SpeakingId { get; set; }
    public int SpeakingEasyId { get; set; }
    public int SpeakingHardId { get; set; }
    public int ReadingId { get; set; }
    public int ReadingEasyId { get; set; }
    public int ReadingHardId { get; set; }
    public int WritingId { get; set; }
    public int WritingEasyId { get; set; }
    public int WritingHardId { get; set; }
}