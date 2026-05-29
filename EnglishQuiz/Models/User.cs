using EnglishQuiz.Models.Enums;

namespace EnglishQuiz.Models;

public class User
{
    public int Id { get; set; } = 0;
    public UserRoles Role { get; set; } = UserRoles.Guest;
    public string Email { get; set; } = string.Empty;
}