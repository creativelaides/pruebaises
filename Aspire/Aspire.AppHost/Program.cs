using System.Diagnostics;
using Aspire.Hosting.Lifecycle;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// ── Parámetros sensibles (se solicitan en la primera ejecución) ──────────
var dbPassword      = builder.AddParameter("db-password", secret: true);
var smtpPassword    = builder.AddParameter("smtp-password", secret: true);
var socrataToken    = builder.AddParameter("socrata-token", secret: true);
var apiPassword     = builder.AddParameter("api-password", secret: true);
var apiSecret       = builder.AddParameter("api-secret", secret: true);
var seedAdminPwd    = builder.AddParameter("seed-admin-password", secret: true);
var seedClientPwd   = builder.AddParameter("seed-client-password", secret: true);
// pgAdmin re-habilitado: bug #17356 corregido en Aspire 13.4.3

// ── Limpieza de volúmenes (equivalente a compose down -v) ────────────────
// Elimina los volúmenes nombrados ANTES de cada arranque para garantizar
// que PostgreSQL siempre inicia con datos limpios (evita conflictos de versión).
builder.Services.AddLifecycleHook<VolumeCleanupHook>();

// ── Orden de arranque ────────────────────────────────────────────────────
// 1. PostgreSQL (container) → healthy
// 2. DefaultConnection (database) → disponible
// 3. API .NET → aplica migraciones + seed → healthy en /health
// 4. Angular → arranca solo cuando la API está healthy

// ── PostgreSQL ──────────────────────────────────────────────────────────
var postgres = builder.AddPostgres("postgres", password: dbPassword)
    .WithDataVolume("postgreises-data")
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050));

// Nombre "DefaultConnection" para que la env var sea ConnectionStrings__DefaultConnection
// y mantener compatibilidad con configuration.GetConnectionString("DefaultConnection")
var db = postgres.AddDatabase("DefaultConnection");

// ── API .NET ─────────────────────────────────────────────────────────────
// WaitFor(db): no arranca hasta que PostgreSQL esté healthy y el DB exista.
// WithHttpHealthCheck: Aspire marca la API como "healthy" solo cuando /health
// responde 200, es decir, DESPUÉS de que las migraciones y el seed terminan.
var api = builder.AddProject<TarifasElectricas_Api>("api")
    .WithReference(db)
    .WithEnvironment("Socrata__AppToken",    socrataToken)
    .WithEnvironment("APIPassword",         apiPassword)
    .WithEnvironment("APISecretPassword",   apiSecret)
    .WithEnvironment("Token",               socrataToken)
    .WithEnvironment("SeedUsers__Admin__Password",    seedAdminPwd)
    .WithEnvironment("SeedUsers__Client__Password",   seedClientPwd)
    .WithEnvironment("Email__Password",      smtpPassword)
    .WaitFor(db)
    .WithHttpHealthCheck("/health");

// ── Angular Frontend ────────────────────────────────────────────────────
// WaitFor(api): Angular NO arranca hasta que la API esté healthy (/health → 200).
// Garantiza que la base de datos, migraciones y seed están listos primero.
var frontend = builder.AddJavaScriptApp("frontend", "../../Frontend")
    .WithRunScript("start")
    .WithHttpEndpoint(targetPort: 4200, name: "http")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpHealthCheck("/");

// Pasamos la URL del frontend a la API para configurar CORS dinámicamente
api.WithEnvironment("AllowedOrigins__Frontend", frontend.GetEndpoint("http"));

builder.Build().Run();

// ── Lifecycle Hook: limpieza de volúmenes antes de cada arranque ─────────
/// <summary>
/// Elimina los volúmenes Docker nombrados antes de que Aspire arranque los contenedores.
/// Equivalente a ejecutar "docker compose down -v" antes de "docker compose up".
/// Garantiza una base de datos limpia en cada ejecución de desarrollo.
/// Los archivos .cs de migraciones NO se eliminan: son código fuente necesario para MigrateAsync.
/// </summary>
file sealed class VolumeCleanupHook : IDistributedApplicationLifecycleHook
{
    // Volúmenes Docker a eliminar antes de cada arranque.
    private static readonly string[] VolumesToClean = ["postgreises-data"];

    public async Task BeforeStartAsync(
        DistributedApplicationModel appModel,
        CancellationToken cancellationToken)
    {
        foreach (var volume in VolumesToClean)
        {
            var psi = new ProcessStartInfo("docker", $"volume rm {volume} --force")
            {
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
            };
            using var process = Process.Start(psi);
            if (process is not null)
                await process.WaitForExitAsync(cancellationToken);
            // Exit code ignorado: si el volumen no existe no hace nada.
        }
    }
}