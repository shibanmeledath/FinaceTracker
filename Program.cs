using FinanceTracker.Components;
using FinanceTracker.Data;
using FinanceTracker.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Handle PostgreSQL URI format (common on Render/Neon)
if (connectionString != null && (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
{
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');
    
    connectionString = $"Host={databaseUri.Host};" +
                      $"Port={databaseUri.Port};" +
                      $"Database={databaseUri.AbsolutePath.TrimStart('/')};" +
                      $"Username={userInfo[0]};" +
                      $"Password={userInfo[1]};" +
                      "SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<FinanceService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");

// ❌ Do NOT use HTTPS redirect on Render
// Render already provides HTTPS

app.UseAntiforgery();
app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();