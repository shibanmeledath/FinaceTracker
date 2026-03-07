using FinanceTracker.Components;
using FinanceTracker.Data;
using FinanceTracker.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString != null && (connectionString.Contains("://")))
{
    // Robust URI parsing for postgres:// and postgresql://
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');
    var port = databaseUri.Port == -1 ? 5432 : databaseUri.Port;
    var database = databaseUri.AbsolutePath.TrimStart('/');
    
    // Clean database name if it has query parameters
    if (database.Contains("?")) database = database.Split('?')[0];

    connectionString = $"Host={databaseUri.Host};" +
                      $"Port={port};" +
                      $"Database={database};" +
                      $"Username={userInfo[0]};" +
                      $"Password={(userInfo.Length > 1 ? userInfo[1] : "")};" +
                      "SSL Mode=Require;Trust Server Certificate=true;Include Error Detail=true";
}

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<FinanceService>();

var app = builder.Build();

// 💡 Automatic Migration: Ensures schema is created on Render
using (var scope = app.Services.CreateScope())
{
    try 
    {
        var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
    }
}

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