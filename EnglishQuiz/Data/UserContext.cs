using EnglishQuiz.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EnglishQuiz.Data;

public class UserContext : User
{
    public bool IsAuthenticated => Id > 0;
}

public static class HighPerformanceUserContextProvider
{
    public static async Task<UserContext> HydrateAsync(AuthenticationStateProvider authProvider)
    {
        UserContext context = new();
        AuthenticationState authState = await authProvider.GetAuthenticationStateAsync();
        ClaimsPrincipal user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            string? idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out int id))
                context.Id = id;
            context.Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
        return context;
    }
}