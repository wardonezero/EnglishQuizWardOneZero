namespace EnglishQuiz.Models;

public record SpeechConfiguration
{
    public string Key1 { get; set; } = string.Empty;
    public string Key2 { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string[] Voices { get; set; } = [];
}