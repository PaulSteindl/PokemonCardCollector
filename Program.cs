using PokemonCardCollector.Components;
using PokemonCardCollector.Models;
using PokemonCardCollector.Repositories;
using PokemonCardCollector.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework Core with SQLite provider
builder.Services.AddDbContext<PokemonCardDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository in dependency injection container (Scoped lifetime - one per HTTP request)
builder.Services.AddScoped<ICardRepository, CardRepository>();

// Register HTTP client for Pokemon Card API integration
// Typed client ensures proper dependency injection and configuration
builder.Services.AddHttpClient<IPokemonCardApiService, PokemonCardApiService>(client =>
{
    // Configure default timeout for API requests (30 seconds)
    client.Timeout = TimeSpan.FromSeconds(30);
    // BaseAddress is set in the service constructor from TCGdex API
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
