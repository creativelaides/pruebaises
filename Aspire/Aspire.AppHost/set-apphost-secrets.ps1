$ErrorActionPreference = "Stop"

$projectPath = Join-Path $PSScriptRoot "Aspire.AppHost.csproj"

if (-not (Test-Path $projectPath)) {
    throw "No se encontro el proyecto AppHost en: $projectPath"
}

$secrets = [ordered]@{
    "Parameters:db-password"           = "HelloWorldHorse9876*"
    "Parameters:smtp-password"        = "mdzs xerv iifo mjzo"
    "Parameters:socrata-token"        = "nJXhGvrlKrtooXNqMKCzgBogk"
    "Parameters:api-password"         = "a9seyji8zx5vf4if82jofuq66"
    "Parameters:api-secret"           = "4qcmgkago2mbnc07h0227xwt306cnneq7f7qarduvwgfnwr12w"
    "Parameters:seed-admin-password"  = "HelloWorldHorse9876*"
    "Parameters:seed-client-password" = "HelloWorldHorse9876*"
    "Parameters:pgadmin-password"     = "HelloHorseWorld1234+"
}

foreach ($entry in $secrets.GetEnumerator()) {
    dotnet user-secrets set $entry.Key $entry.Value --project $projectPath | Out-Null
}

Write-Host "AppHost secrets configurados: $($secrets.Count)"
Write-Host "Proyecto: $projectPath"