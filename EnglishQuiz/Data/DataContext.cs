using EnglishQuiz.Models;
using EnglishQuiz.Models.Speaking;
using EnglishQuiz.Models.Writing;
using Microsoft.EntityFrameworkCore;

namespace EnglishQuiz.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<SingleChoiceQuestion> SingleChoiceQuestions { get; set; }
    public DbSet<SingleChoiceQuestionAnswer> SingleChoiceQuestionAnswer { get; set; }
    public DbSet<SingleChoiceQuestionStudentAnswer> SingleChoiceQuestionStudentAnswer { get; set; }

    public DbSet<MultiChoiceQuestion> MultiChoiceQuestions { get; set; }
    public DbSet<MultiChoiceQuestionAnswer> MultiChoiceQuestionAnswer { get; set; }

    public DbSet<Listening> Listenings { get; set; }

    public DbSet<ListenRepeat> ListenRepeat { get; set; }
    public DbSet<Conversation> Conversations { get; set; }

    public DbSet<Reading> Readings { get; set; }

    public DbSet<BuildSentence> BuildSentence { get; set; }
    public DbSet<WriteEssay> WriteEssay { get; set; }
    public DbSet<WriteDiscussion> WriteDiscussion { get; set; }

    public DbSet<Quiz> Quizzes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        Type[] answerModels = [typeof(SingleChoiceQuestionAnswer), typeof(SingleChoiceQuestionStudentAnswer), typeof(MultiChoiceQuestionAnswer), typeof(BuildSentenceAnswer)];
        foreach (Type? entityType in answerModels)
        {
            modelBuilder.Entity(entityType)
                .Property("Id")
                .ValueGeneratedNever();
        }
    }
}