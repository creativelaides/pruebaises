# AGENT.md - Project Context

## Directory Overview
Repositorio de una prueba tecnica full-stack para monitorear tarifas de energia electrica en Colombia con fuente oficial (`datos.gov.co`).

Estado actual:
- Backend implementado en .NET 10 (Clean Architecture, CQRS, Identity, ETL, Email).
- Frontend implementado en Angular 21 (modulos de auth, tarifas, facturas, ETL y admin).
- Base de datos desplegable por Docker (PostgreSQL + pgAdmin).

## Arquitectura y modulos
- **Domain**: entidades, value objects e invariantes de negocio.
- **Application**: casos de uso, validaciones, contratos y mapeos.
- **Infrastructure (Persistence)**: `TarifasElectricas.Persistence` con EF Core, repositorios y migraciones.
- **Infrastructure (Identity)**: `TarifasElectricas.Identity` con usuarios, roles, politicas y seed.
- **Infrastructure (Integration)**: `TarifasElectricas.Infrastructure` con ETL Socrata y servicio SMTP.
- **API**: controladores REST + middlewares + OpenAPI/Scalar.
- **Frontend**: Angular standalone con rutas por feature.
- **Tests**: xUnit + NSubstitute para domain/application/infra.

## Key files and folders
- `Backend/TarifasElectricas.slnx`: solucion principal.
- `Backend/src/API/TarifasElectricas.Api/Program.cs`: bootstrap de API, CORS, auth y DI.
- `Backend/src/Core/TarifasElectricas.Domain/`: capa Domain.
- `Backend/src/Core/TarifasElectricas.Application/`: capa Application.
- `Backend/src/Infrastructure/TarifasElectricas.Persistence/`: persistencia de negocio.
- `Backend/src/Infrastructure/TarifasElectricas.Identity/`: autenticacion/autorizacion.
- `Backend/src/Infrastructure/TarifasElectricas.Infrastructure/`: ETL y correo.
- `Backend/src/Test/TarifasElectricas.Test/`: pruebas unitarias.
- `Frontend/src/app/`: features y core del frontend.
- `Database/docker-compose.yml`: servicios PostgreSQL y pgAdmin.
- `Summaries/APPLICATION_STRUCTURE_SUMMARY.md`: resumen Application.
- `Summaries/DOMAIN_STRUCTURE_SUMMARY.md`: resumen Domain.
- `Skills/PRUEBA_TECNICA_FULLSTACK.md`: enunciado original.

## Comandos utiles
```bash
cd Database
copy env.template .env
docker compose --env-file .env up -d
```

```bash
cd Backend
dotnet restore
dotnet build
dotnet test
dotnet run --project src/API/TarifasElectricas.Api
```

```bash
cd Frontend
npm install
npm start
```

## Notas
- Cambiar secretos de `appsettings.example.json` antes de uso real.
- Existe `set-user-secrets.ps1` para facilitar configuracion local de secretos.

