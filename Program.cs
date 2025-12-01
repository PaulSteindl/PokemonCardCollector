using PokemonCardCollector.Components;
using PokemonCardCollector.Models;
using PokemonCardCollector.Repositories;
using PokemonCardCollector.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure structured logging for better diagnostics and error tracking
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add Entity Framework Core with SQLite provider
// Connection string is configured via appsettings.json and environment-specific overrides
builder.Services.AddDbContext<PokemonCardDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
        throw new InvalidOperationException("DefaultConnection string is not configured in appsettings.json");

    options.UseSqlite(connectionString);

    // Enable detailed error messages in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

// Register repository in dependency injection container
// Scoped lifetime - one instance per HTTP request, appropriate for database context usage
builder.Services.AddScoped<ICardRepository, CardRepository>();

// Register business logic services
// Scoped lifetime - coordinates repository and API service calls per request
builder.Services.AddScoped<ICardCollectionService, CardCollectionService>();

// Register image URL formatting service
// Singleton lifetime - stateless utility service with no dependencies
builder.Services.AddSingleton<IImageUrlService, ImageUrlService>();

// Register HTTP client for Pokemon Card API integration using typed client pattern
// This provides automatic dependency injection of HttpClient with proper lifecycle management
builder.Services.AddHttpClient<IPokemonCardApiService, PokemonCardApiService>(client =>
{
    // Configure default timeout for API requests (30 seconds)
    client.Timeout = TimeSpan.FromSeconds(30);
    // Set User-Agent header for API identification
    client.DefaultRequestHeaders.Add("User-Agent", "PokemonCardCollector/1.0");
});

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // In production, use exception handler to display user-friendly error page
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // Enable HSTS (HTTP Strict-Transport-Security) for 30 days
    // HSTS helps prevent man-in-the-middle attacks by forcing HTTPS
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
