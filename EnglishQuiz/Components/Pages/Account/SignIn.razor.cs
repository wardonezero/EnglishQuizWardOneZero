using Microsoft.AspNetCore.Components;

namespace EnglishQuiz.Components.Pages.Account;

public partial class SignIn
{
    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }
    private string CurrentReturnUrl => string.IsNullOrEmpty(ReturnUrl) ? "/" : ReturnUrl;
}