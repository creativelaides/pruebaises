# AGENT.md - Project Context

## Directory Overview

Repositorio de una prueba tecnica para monitorear y visualizar tarifas de energia electrica en Colombia usando datos de `datos.gov.co`.

Estado actual:
- Backend en .NET 10 con Clean Architecture, CQRS y validaciones.
- Frontend aun no implementado (solo carpeta base).
- Persistencia planificada en PostgreSQL.

## Arquitectura y Modulos

- **Domain**: entidades, value objects y reglas de negocio.
- **Application**: casos de uso (commands/queries), validaciones, mapeos y contratos.
- **Infrastructure**: persistencia (proyecto `TarifasElectricas.Persitence`).
- **Tests**: xUnit + NSubstitute para Domain y Application.

## Key Files and Folders

- `Backend/TarifasElectricas.slnx`: solucion principal .NET.
- `Backend/src/Core/TarifasElectricas.Domain/`: capa de dominio.
- `Backend/src/Core/TarifasElectricas.Application/`: casos de uso y contratos.
- `Backend/src/Infrastructure/TarifasElectricas.Persitence/`: infraestructura de persistencia.
- `Backend/src/Test/TarifasElectricas.Test/`: pruebas unitarias.
- `Summaries/APPLICATION_STRUCTURE_SUMMARY.md`: resumen actualizado de Application.
- `Summaries/DOMAIN_STRUCTURE_SUMMARY.md`: resumen actualizado de Domain.
- `Skills/Diagrams/`: diagramas Mermaid.
- `Skills/Visuals/`: recursos visuales.
- `PRUEBA_TECNICA_FULLSTACK.md`: enunciado original de la prueba.

## Comandos utiles (Backend)

```bash
cd Backend
dotnet restore
dotnet build
dotnet test
```

## Notas

- `Backend/src/API` existe pero no contiene proyecto aun.
- El frontend y la base de datos estan planificados, no implementados en este repo.

