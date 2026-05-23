using EnglishQuiz.Models.Interfaces;

namespace EnglishQuiz.Models;

public record RIdName : IIdName { public int Id { get; set; } = 0; public string Name { get; set; } = string.Empty; }

public record FileItem(string Name, string FullPath, bool IsFolder);
