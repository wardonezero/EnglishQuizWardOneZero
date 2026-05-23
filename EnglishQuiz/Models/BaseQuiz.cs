using EnglishQuiz.Models.Enums;
using EnglishQuiz.Models.Interfaces;

namespace EnglishQuiz.Models;

public class BaseQuiz : IQuizListItem
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public Difficulties Difficulty { get; set; } = Difficulties.Medium;
    public int DisplayOrder { get; set; } = 0;
    public bool Published { get; set; } = false;
}