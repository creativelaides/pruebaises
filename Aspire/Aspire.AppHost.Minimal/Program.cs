using Projects;

// AppHost MÍNIMO para debuggear bug #17356
// Solo PostgreSQL + API, SIN Angular ni pgAdmin
// Si ESTE funciona sin timeout, el problema es el frontend JavaScript

var builder = DistributedApplication.CreateBuilder(args);

// ── Parámetros sensibles ──────────
var dbPassword = builder.AddParameter("db-password", secret: true);
var smtpPassword = builder.AddParameter("smtp-password", secret: true);
var socrataToken = builder.AddParameter("socrata-token", secret: true);
var apiPassword = builder.AddParameter("api-password", secret: true);
var apiSecret = builder.AddParameter("api-secret", secret: true);
var seedAdminPwd = builder.AddParameter("seed-admin-password", secret: true);
var seedClientPwd = builder.AddParameter("seed-client-password", secret: true);

// ── PostgreSQL ───────────────────
var postgres = builder.AddPostgres("postgres", password: dbPassword)
    .WithDataVolume("postgreises-data-minimal");

var db = postgres.AddDatabase("DefaultConnection");

// ── API .NET ─────────────────────
var api = builder.AddProject<TarifasElectricas_Api>("api")
    .WithReference(db)
    .WithEnvironment("Socrata__AppToken", socrataToken)
    .WithEnvironment("APIPassword", apiPassword)
    .WithEnvironment("APISecretPassword", apiSecret)
    .WithEnvironment("Token", socrataToken)
    .WithEnvironment("SeedUsers__Admin__Password", seedAdminPwd)
    .WithEnvironment("SeedUsers__Client__Password", seedClientPwd)
    .WithEnvironment("Email__Password", smtpPassword)
    .WaitFor(db);

builder.Build().Run();
