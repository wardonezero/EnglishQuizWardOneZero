using EnglishQuiz;
using EnglishQuiz.Components;
using EnglishQuiz.Data;
using EnglishQuiz.Models.Enums;
using EnglishQuiz.Services;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

ConfigurationManager configuration = builder.Configuration;
string connectionString = configuration.GetConnectionString("SQLite") ?? throw new InvalidOperationException("Connection string not found");
builder.Services.AddDbContextPool<DataContext>(option => option.UseSqlite(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(options =>
  {
      options.Cookie.Name = "AuthCookie";
      options.LoginPath = "/account/sign-in";
      options.LogoutPath = "/account/sign-out";
      options.AccessDeniedPath = "/account/sign-in";
      options.ExpireTimeSpan = TimeSpan.FromHours(24);
      options.SlidingExpiration = true;
      options.Cookie.HttpOnly = true;
      options.Cookie.SameSite = SameSiteMode.Lax;
      options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
  })
  .AddGoogleOpenIdConnect(options =>
  {
      options.ClientId = configuration["Authentication:Google:ClientId"]!;
      options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
      options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.CallbackPath = "/signin-google";
  });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Student", policy => policy.RequireAssertion(ctx => Enum.TryParse<UserRoles>(ctx.User.FindFirstValue(ClaimTypes.Role), out UserRoles r) && r >= UserRoles.Student))
    .AddPolicy("Moderator", policy => policy.RequireAssertion(ctx => Enum.TryParse<UserRoles>(ctx.User.FindFirstValue(ClaimTypes.Role), out UserRoles r) && r >= UserRoles.Moderator))
    .AddPolicy("Administrator", policy => policy.RequireAssertion(ctx => Enum.TryParse<UserRoles>(ctx.User.FindFirstValue(ClaimTypes.Role), out UserRoles r) && r == UserRoles.Administrator));

builder.Services.AddScoped<UserContext>(sp =>
{
    AuthenticationStateProvider authProvider = sp.GetRequiredService<AuthenticationStateProvider>();
    return HighPerformanceUserContextProvider.HydrateAsync(authProvider).GetAwaiter().GetResult();
});

builder.Services.AddScoped(typeof(GenericService<>));
builder.Services.AddScoped<FileManagerService>();
builder.Services.AddScoped<AudioService>();
builder.Services.AddScoped<QuizService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

string dataPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "Data"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(dataPath),
    RequestPath = "/Data"
});

app.MapGet("/console", () => Results.Redirect("/console/dashboard"));

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAuthEndpoints();

app.Run();