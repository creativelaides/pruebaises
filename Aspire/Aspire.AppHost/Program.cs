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
// pgAdmin deshabilitado temporalmente — bug #17356: health check timeout
// corrompe el backchannel JSON-RPC del CLI. Re-habilitar con Aspire 13.4+.
// var pgAdminPassword = builder.AddParameter("pgadmin-password", secret: true);

// ── PostgreSQL ──────────────────────────────────────────────────────────
var postgres = builder.AddPostgres("postgres", password: dbPassword)
    .WithDataVolume("postgreises-data");
    // .WithPgAdmin deshabilitado — bug #17356 (health check 120s timeout mata backchannel)

// Nombre "DefaultConnection" para que la env var sea ConnectionStrings__DefaultConnection
// y mantener compatibilidad con configuration.GetConnectionString("DefaultConnection")
var db = postgres.AddDatabase("DefaultConnection");

// ── API .NET ─────────────────────────────────────────────────────────────
var api = builder.AddProject<TarifasElectricas_Api>("api")
    .WithReference(db)
    .WithEnvironment("Socrata__AppToken",    socrataToken)
    .WithEnvironment("APIPassword",         apiPassword)
    .WithEnvironment("APISecretPassword",   apiSecret)
    .WithEnvironment("Token",               socrataToken)
    .WithEnvironment("SeedUsers__Admin__Password",    seedAdminPwd)
    .WithEnvironment("SeedUsers__Client__Password",   seedClientPwd)
    .WithEnvironment("Email__Password",      smtpPassword)
    .WaitFor(db);

// ── Angular Frontend (npm/pnpm/yarn start via AddJavaScriptApp) ────────
// Dejamos que Aspire asigne un puerto dinámico (omitimos targetPort: 4200)
// y lo pase al proceso vía env var "PORT".
var frontend = builder.AddJavaScriptApp("frontend", "../../Frontend")
    .WithRunScript("start")
    .WithHttpEndpoint(name: "http", env: "PORT") 
    .WithReference(api)
    .WaitFor(api)
    .WithHttpHealthCheck("/");

// Pasamos la URL del frontend a la API para configurar CORS dinámicamente
api.WithEnvironment("AllowedOrigins__Frontend", frontend.GetEndpoint("http"));

builder.Build().Run();