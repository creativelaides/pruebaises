<div align="center">
<h1>Prueba ISES - Tarifas Electricas 2026</h1>
<img margin=20px src ="./Skills/Visuals/project-top-image.svg" alt="ISESTopImageDotnetAngularPostgresProject" align="center" height="150px" >
<br>
</div>

Proyecto full-stack para monitorear y visualizar tarifas de energia electrica en Colombia usando datos publicos de `datos.gov.co`. El backend esta implementado en .NET con Clean Architecture y CQRS. El frontend aun no esta implementado en este repositorio (solo existe la carpeta base).

Para mas detalle tecnico y referencias de arquitectura, ver `AGENT.md` y los summaries en `Summaries/`.

**Estructura**
- `Backend/`: solucion .NET 10 (Domain, Application, Persistence, Tests)
- `Summaries/`: documentacion actualizada de capas Domain y Application
- `Frontend/`: placeholder (sin codigo aun)
- `Skills/`: diagramas y recursos visuales

**Tecnologias**
- .NET 10 (SDK 10.0.103)
- xUnit + NSubstitute
- FluentValidation
- Mapster
- WolverineFx
- PostgreSQL (planificado)
- Angular (planificado)

**Quick Start (Backend)**
```bash
cd Backend
dotnet restore
dotnet build
dotnet test
```

**Docs clave**
- `Summaries/APPLICATION_STRUCTURE_SUMMARY.md`
- `Summaries/DOMAIN_STRUCTURE_SUMMARY.md`
