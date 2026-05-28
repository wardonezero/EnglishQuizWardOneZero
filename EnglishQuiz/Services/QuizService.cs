using EnglishQuiz.Data;
using EnglishQuiz.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishQuiz.Services;

public sealed class QuizService(DataContext context)
{
    public async Task<(string Question, string[] MixedAnswers)> GetSingleChoiceQuestionAsync(int id)
    {
        SingleChoiceQuestion questionModel = await context.SingleChoiceQuestions.AsNoTracking()
            .Where(q => q.Id == id).FirstOrDefaultAsync() ??
            throw new InvalidOperationException("GetSingleChoiceQuestionAsync Returned Null");

        string[] incorrect = questionModel.Answers;

        string correct = await context.SingleChoiceQuestionAnswer.AsNoTracking()
            .Where(a => a.Id == id).Select(a => a.Answer).FirstOrDefaultAsync() ??
            throw new InvalidOperationException("GetSingleChoiceQuestionAsync Returned Null");

        string[] mixedAnswers = new string[4];
        mixedAnswers[0] = incorrect[0];
        mixedAnswers[1] = incorrect[1];
        mixedAnswers[2] = incorrect[2];
        mixedAnswers[3] = correct;

        int k;
        string value;
        int n = mixedAnswers.Length;
        while (n > 1)
        {
            n--;
            k = Random.Shared.Next(n + 1);
            value = mixedAnswers[k];
            mixedAnswers[k] = mixedAnswers[n];
            mixedAnswers[n] = value;
        }
        return (questionModel.Question, mixedAnswers);
    }

    public async Task SaveSingleChoiceQuestionStudentAnswerAsync(int questionId, int studentId, string answer, string[] options)
    {
        SingleChoiceQuestionStudentAnswer? record = await context.SingleChoiceQuestionStudentAnswer
            .FirstOrDefaultAsync(a => a.Id == questionId && a.StudentId == studentId);

        if (record == null)
        {
            record = new SingleChoiceQuestionStudentAnswer
            {
                Id = questionId,
                StudentId = studentId
            };
            context.SingleChoiceQuestionStudentAnswer.Add(record);
        }

        string correctAnswer = await context.SingleChoiceQuestionAnswer.AsNoTracking()
            .Where(q => q.Id == questionId).Select(q => q.Answer).FirstOrDefaultAsync() ?? string.Empty;

        record.Answer = answer ?? string.Empty;
        record.CorrectAnswer = correctAnswer;
        record.Options = options;
        await context.SaveChangesAsync();
    }

    public async Task<SingleChoiceQuestionStudentAnswer> GetSingleChoiceQuestionStudentAnswerAsync(int id, int studentId)
    {
        SingleChoiceQuestionStudentAnswer result = await context.SingleChoiceQuestionStudentAnswer.AsNoTracking()
            .Where(q => q.Id == id && q.StudentId == studentId).FirstOrDefaultAsync() ??
            throw new InvalidOperationException("GetSingleChoiceQuestionAnswerAsync Returned Null");

        return result;
    }

    public async Task<SingleChoiceQuestionStudentAnswer[]> GetSingleChoiceQuestionStudentAnswersAsync(int[] ids, int studentId)
    {
        SingleChoiceQuestionStudentAnswer[] result = await context.SingleChoiceQuestionStudentAnswer.AsNoTracking()
            .Where(q => ids.Contains(q.Id) && q.StudentId == studentId).ToArrayAsync();
        return result;
    }
}