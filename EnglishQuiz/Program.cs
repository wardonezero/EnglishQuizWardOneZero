using EnglishQuiz.Components;
using EnglishQuiz.Data;
using EnglishQuiz.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string connectionString = builder.Configuration.GetConnectionString("SQLite")!;

builder.Services.AddDbContext<DataContext>(option => option.UseSqlite(connectionString));

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

app.Run();
