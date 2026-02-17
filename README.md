<div align="center">
<h1>Prueba ISES - Tarifas Electricas 2026</h1>
<img margin=20px src="./Skills/Visuals/project-top-image.svg" alt="ISES top image Dotnet Angular Postgres Project" align="center" height="150px">
<br><br>
</div>

Proyecto full-stack para monitorear, consultar y simular tarifas de energia electrica en Colombia usando datos oficiales de `datos.gov.co`.

## Descripcion detallada
La solucion implementa un flujo completo de negocio:

- **ETL de tarifas**: consume la fuente oficial, transforma datos y registra ejecuciones.
- **API REST**: expone operaciones para tarifas, simulacion de factura, cuenta de usuario y ETL.
- **Autenticacion y autorizacion**: usa ASP.NET Core Identity con roles (`Admin`, `Client`) y politicas.
- **Interfaz web**: frontend Angular con modulos funcionales (`auth`, `tariffs`, `invoices`, `etl`, `admin`).
- **Persistencia**: PostgreSQL para datos del dominio y datos de identidad.
- **Notificaciones**: soporte de envio de correo via SMTP.

## Arquitectura del repositorio
- `Backend/`: solucion .NET 10 con Clean Architecture (`Domain`, `Application`, `Infrastructure`, `API`, `Test`).
- `Frontend/`: aplicacion Angular 21.
- `Database/`: stack Docker para PostgreSQL + pgAdmin.
- `Summaries/`: resumenes tecnicos de capas core.
- `Skills/`: enunciado, diagramas y recursos visuales de apoyo.

## Stack tecnologico
- **Backend**: .NET SDK `10.0.103`, ASP.NET Core, Identity, FluentValidation, Mapster.
- **Frontend**: Angular `21`, TailwindCSS `4`, DaisyUI `5`.
- **Datos**: PostgreSQL `18.2-alpine`, pgAdmin.
- **Testing**: xUnit + NSubstitute (backend), Vitest (frontend).

## Endpoints funcionales principales
- `api/tariffs`: CRUD, consulta por periodo y consulta de ultima tarifa.
- `api/invoices/simulate`: simulacion de factura por consumo.
- `api/etl/run`: ejecucion de ETL.
- `api/email/test`: prueba de envio de correo.
- `api/account/*`: perfil y cambio de contrasena.
- Endpoints de Identity mapeados con `MapIdentityApi<AppUser>()`.

## Puesta en marcha
### 1) Base de datos (Docker)
```bash
cd Database
copy env.template .env
docker compose --env-file .env up -d
```

### 2) Backend
```bash
cd Backend
dotnet restore
dotnet build
dotnet test
dotnet run --project src/API/TarifasElectricas.Api
```

### 3) Frontend
```bash
cd Frontend
npm install
npm start
```

## Configuracion importante
- Revisar `Backend/src/API/TarifasElectricas.Api/appsettings.example.json`.
- Configurar `ConnectionStrings:DefaultConnection`.
- Configurar credenciales SMTP en `Email`.
- Cambiar passwords de `SeedUsers` antes de ejecutar en cualquier entorno real.

## Documentacion tecnica (Summaries)
- [Application Structure Summary](./Summaries/APPLICATION_STRUCTURE_SUMMARY.md)
- [Domain Structure Summary](./Summaries/DOMAIN_STRUCTURE_SUMMARY.md)

## Referencias adicionales
- `AGENT.md`
- `Skills/PRUEBA_TECNICA_FULLSTACK.md`
