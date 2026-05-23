namespace EnglishQuiz.Models.Interfaces;

public interface IListItem : IIdName
{
    int DisplayOrder { get; set; }
    bool Published { get; set; }
}