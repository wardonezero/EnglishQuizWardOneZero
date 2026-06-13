using EnglishQuiz.Data;
using EnglishQuiz.Models;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnglishQuiz;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/account");

        group.MapGet("/sign-in-google", ([FromQuery] string? returnUrl) =>
        {
            AuthenticationProperties properties = new() { RedirectUri = $"/account/process-sign-in?returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}" };
            return Results.Challenge(properties, [GoogleOpenIdConnectDefaults.AuthenticationScheme]);
        });

        group.MapGet("/process-sign-in", async ([FromQuery] string? returnUrl, HttpContext httpContext, [FromServices] DataContext database) =>
        {
            // The user is already authenticated via Google OIDC at this point
            if (httpContext.User.Identity?.IsAuthenticated != true)
            {
                return Results.Redirect("/account/sign-in?error=NotAuthenticated");
            }

            string? email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Results.Redirect("/account/sign-in?error=NoEmail");

            // High-Performance Trackingless DB Lookup to find internal primitives
            User? user = await database.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new() { Email = email, Role = Models.Enums.UserRoles.Student };
                database.Users.Add(user);
                await database.SaveChangesAsync();
            }

            // Re-issue the cookie with your native database Primary Key ID included
            List<Claim> localClaims =
            [
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString()),
            ];

            ClaimsIdentity claimsIdentity = new(localClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Re-sign-in locally with the updated database ID claim
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1) });

            return !string.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl) ? Results.Redirect(returnUrl) : Results.Redirect("/");
        });

        group.MapPost("/sign-out", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/account/sign-in");
        });
    }

    private static bool IsLocalUrl(string url)
    {
        return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\')))
               || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
    }
}