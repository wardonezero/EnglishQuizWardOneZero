using EnglishQuiz.Models.Enums;

namespace EnglishQuiz.Models.Interfaces;

public interface IQuizListItem : IListItem
{
    Difficulties Difficulty { get; set; }
}