using Hangfire;
using Hangfire.SqlServer;
using HangfireCleanup.Data;
using HangfireCleanup.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


var builder = WebApplication.CreateBuilder(args);

// Konfiguracija baze podataka
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrujte CleanupService
builder.Services.AddScoped<CleanupService>();

// Konfiguracija Hangfire-a
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();

var app = builder.Build();

// Pokrenite Hangfire Dashboard
app.UseHangfireDashboard();

// Zakažite dnevni posao
using (var scope = app.Services.CreateScope())
{
    var cleanupService = scope.ServiceProvider.GetRequiredService<CleanupService>();
    RecurringJob.AddOrUpdate("CleanupJob", () => cleanupService.ObrisiStareKorisnike(), Cron.Daily);

}

app.Run();

