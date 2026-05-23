using EnglishQuiz.Models.Interfaces;

namespace EnglishQuiz.Models;

public class QuizType : IListItem
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public bool Published { get; set; } = false;
}