$ErrorActionPreference = "Stop"

$projectPath = Join-Path $PSScriptRoot "src/API/TarifasElectricas.Api/TarifasElectricas.Api.csproj"

if (-not (Test-Path $projectPath)) {
    throw "No se encontro el proyecto API en: $projectPath"
}

$secrets = [ordered]@{
    "ConnectionStrings:DefaultConnection" = "Host=127.0.0.1;Port=5588;Database=postgreIses;Username=johndoe;Password=HelloWorldHorse9876*;Timeout=15;"

    "Socrata:AppToken" = "nJXhGvrlKrtooXNqMKCzgBogk"

    "APIPassword" = "a9seyji8zx5vf4if82jofuq66"
    "APISecretPassword" = "4qcmgkago2mbnc07h0227xwt306cnneq7f7qarduvwgfnwr12w"
    "Token" = "nJXhGvrlKrtooXNqMKCzgBogk"

    "SeedUsers:Admin:Username" = "creativelaides@gmail.com"
    "SeedUsers:Admin:Email" = "creativelaides@gmail.com"
    "SeedUsers:Admin:Password" = "HelloWorldHorse9876*"
    "SeedUsers:Admin:FirstName" = "Jose"
    "SeedUsers:Admin:LastName" = "Velaides"
    "SeedUsers:Admin:JobTitle" = "Developer"
    "SeedUsers:Admin:Area" = "IT"

    "SeedUsers:Client:Username" = "stephajaimes@gmail.com"
    "SeedUsers:Client:Email" = "stephajaimes@gmail.com"
    "SeedUsers:Client:Password" = "HelloWorldHorse9876*"
    "SeedUsers:Client:FirstName" = "Stephania"
    "SeedUsers:Client:LastName" = "Jaimes"
    "SeedUsers:Client:JobTitle" = "Psicologa"
    "SeedUsers:Client:Area" = "Gestion Humana"

    "Email:Host" = "smtp.gmail.com"
    "Email:Port" = "587"
    "Email:User" = "develaides@gmail.com"
    "Email:Password" = "mdzs xerv iifo mjzo"
    "Email:FromEmail" = "develaides@gmail.com"
    "Email:FromName" = "PruebaIses"
    "Email:UseSsl" = "true"
}

foreach ($entry in $secrets.GetEnumerator()) {
    dotnet user-secrets set $entry.Key $entry.Value --project $projectPath | Out-Null
}

Write-Host "Secrets configurados: $($secrets.Count)"
Write-Host "Proyecto: $projectPath"
