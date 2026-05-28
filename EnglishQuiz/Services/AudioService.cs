using EnglishQuiz.Models;
using Microsoft.CognitiveServices.Speech;

namespace EnglishQuiz.Services;

public sealed class AudioService(IConfiguration configuration, IWebHostEnvironment environment)
{
    private readonly string _dataRootPath = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "Data"));
    private readonly SpeechConfiguration _speechConfiguration = configuration.GetSection("SpeechConfiguration").Get<SpeechConfiguration>() ?? throw new Exception("Could not find the SpeechConfiguration");
    public async Task<string> GenerateAudioAsync(int id, string name, string section, string quizType, string text, string? voice)
    {
        if (id <= 0 || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(quizType) || string.IsNullOrEmpty(text))
            throw new InvalidOperationException("Invalid Parameter passed into the GenerateAudioAsync");

        string testType = "EnglishQuiz";

        SpeechConfig config = SpeechConfig.FromSubscription(_speechConfiguration.Key1, _speechConfiguration.Location);
        config.SpeechSynthesisVoiceName = voice ?? _speechConfiguration.Voices[0];
        config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio16Khz32KBitRateMonoMp3);

        using SpeechSynthesizer synthesizer = new(config, audioConfig: null);
        SpeechSynthesisResult result = await synthesizer.SpeakTextAsync(text);

        if (result.Reason != ResultReason.SynthesizingAudioCompleted)
        {
            SpeechSynthesisCancellationDetails cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            throw new InvalidOperationException($"TTS failed [{cancellation.Reason}]: {cancellation.ErrorDetails}");
        }

        string folderPath = Path.Combine(_dataRootPath, testType, section, quizType);
        Directory.CreateDirectory(folderPath);

        string fileName = $"{id}_{name}.mp3";
        string filePath = Path.Combine(folderPath, fileName);

        await File.WriteAllBytesAsync(filePath, result.AudioData);
        return Path.Combine("Data", testType, section, quizType, fileName).Replace("\\", "/");
    }
}