namespace EnglishQuiz.Models.Enums;

[Flags]
public enum UserRoles
{
    Guest = 0b_0001,
    Student = 0b_0010,
    Moderator = 0b_0100,
    Administrator = 0b_1000
}