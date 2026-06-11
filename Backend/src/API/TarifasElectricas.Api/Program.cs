using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TarifasElectricas.Application;
using TarifasElectricas.Api.Middlewares;
using TarifasElectricas.Identity;
using TarifasElectricas.Identity.Models;
using TarifasElectricas.Identity.Seed;
using TarifasElectricas.Infrastructure;
using TarifasElectricas.Persistence;
using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);
const string CorsPolicyName = "FrontendDev";

builder.AddServiceDefaults();

// TariffDbContext inyecta IAppUserService (Scoped) para auditoría.
// AddNpgsqlDbContext usa DbContext pooling internamente, y el pool resuelve
// desde el root provider (no puede resolver servicios Scoped).
// Solución: registrar con AddDbContext estándar (sin pool) + Enrich para telemetría.
builder.Services.AddDbContext<TariffDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.EnrichNpgsqlDbContext<TariffDbContext>();

// AppIdentityDbContext no tiene dependencias Scoped: puede usar AddNpgsqlDbContext con pool.
builder.AddNpgsqlDbContext<AppIdentityDbContext>("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        var allowedOrigins = new List<string>
        {
            "http://localhost:4200",
            "http://127.0.0.1:4200"
        };

        var aspireFrontend = builder.Configuration["AllowedOrigins:Frontend"];
        if (!string.IsNullOrEmpty(aspireFrontend))
        {
            allowedOrigins.Add(aspireFrontend.TrimEnd('/'));
        }

        policy
            .WithOrigins(allowedOrigins.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddScoped<ApiExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.EnablePersistentAuthentication();
    });
}

app.UseApiExceptionHandling();
app.UseHttpsRedirection();
app.UseCors(CorsPolicyName);

app.MapDefaultEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<AppUser>();

// ── Migraciones y Seed ────────────────────────────────────────────
// MigrateAsync aplica las migraciones incrementalmente.
// El VolumeCleanupHook elimina el volumen antes de cada arranque,
// garantizando una DB vacía donde las migraciones se aplican sin conflictos.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Aplicando migraciones...");

        var tariffContext = services.GetRequiredService<TariffDbContext>();
        await tariffContext.Database.MigrateAsync();

        var identityContext = services.GetRequiredService<AppIdentityDbContext>();
        await identityContext.Database.MigrateAsync();

        logger.LogInformation("Migraciones completadas. Iniciando seeding...");
        await IdentitySeeder.SeedAsync(app.Services, app.Configuration);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al migrar o sembrar la base de datos.");
        throw;
    }
}

app.Run();
