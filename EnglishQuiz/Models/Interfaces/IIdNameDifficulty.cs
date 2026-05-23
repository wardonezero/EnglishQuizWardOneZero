using EnglishQuiz.Models.Enums;

namespace EnglishQuiz.Models.Interfaces;

public interface IIdNameDifficulty : IIdName
{
    Difficulties Difficulty { get; set; }
}