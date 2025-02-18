using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TeddySwap.Data;
using TeddySwap.Data.Services;
using TeddySwap.UI.Components;
using TeddySwap.UI.Services;
using TeddySwap.UI.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<TeddySwapDbContext>(options =>
{
    options
    .UseNpgsql(
        builder.Configuration
        .GetConnectionString("TeddySwapContext"),
            x =>
            {
                x.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    builder.Configuration.GetConnectionString("TeddySwapContextSchema")
                );
            }
        );
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<CacheService>();
builder.Services.AddSingleton<CardanoDataService>();
builder.Services.AddSingleton<BlockDataService>();
builder.Services.AddSingleton<YieldFarmingDataService>();

// Workers
builder.Services.AddHostedService<CardanoWorker>();

// MudBlazor
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Use((context, next) =>
{
    // Remove the X-Frame-Options header
    context.Response.Headers.Remove("X-Frame-Options");

    // Set Content-Security-Policy for frame-ancestors to allow all domains ('*')
    // Note: Adjust the policy as needed for your security requirements
    context.Response.Headers.Append("Content-Security-Policy", "frame-ancestors *");

    return next.Invoke();
});

app.Run();
