# Application Layer - Estructura Actualizada

## Vista general
La capa `Application` orquesta los casos de uso del sistema sin depender de infraestructura concreta. Implementa CQRS (comandos y consultas), validaciones por caso de uso, contratos para persistencia/servicios y mapeos con Mapster.

```text
Backend/src/Core/TarifasElectricas.Application/
├── Contracts/
│   ├── Identity/IAppUserService.cs
│   ├── Persistence/IUnitOfWork.cs
│   ├── Repositories/
│   │   ├── Generic/IRepository.cs
│   │   ├── ICompanyRepository.cs
│   │   ├── IElectricityTariffRepository.cs
│   │   └── IEtlLogRepository.cs
│   └── Services/
│       ├── IEtlService.cs
│       └── IEmailService.cs
├── Exceptions/
│   ├── ApplicationCaseException.cs
│   └── HandlerGuard.cs
├── Mapping/MapsterConfig.cs
├── UseCases/
│   ├── Commands/
│   │   ├── CreateTariff/
│   │   ├── UpdateTariff/
│   │   └── DeleteTariff/
│   └── Queries/
│       ├── GetAllTariffs/
│       ├── GetLatestTariff/
│       ├── GetTariffById/
│       ├── GetTariffByPeriod/
│       └── SimulateInvoice/
└── DependencyInjectionApplication.cs
```

## Contratos
- `IRepository<T>`: operaciones genericas para entidades.
- `IElectricityTariffRepository`, `ICompanyRepository`, `IEtlLogRepository`: acceso especializado por agregado.
- `IUnitOfWork`: coordinacion transaccional con `SaveChangesAsync`.
- `IEtlService`: ejecucion de ETL y lectura de logs.
- `IEmailService`: envio de correos desde casos de uso.
- `IAppUserService`: acceso al usuario actual para auditoria/reglas.

## Casos de uso (CQRS)
- Commands: `CreateTariffCommand`, `UpdateTariffCommand`, `DeleteTariffCommand`.
- En comandos se usa estructura `Command`, `Validator`, `Handler` y `Response`.
- Queries: `GetAllTariffsQuery`, `GetLatestTariffQuery`, `GetTariffByIdQuery`, `GetTariffByPeriodQuery`, `SimulateInvoiceQuery`.
- En queries se usa `Query`, `Handler` y `Response` (con validacion cuando aplica).

## Manejo de errores y guardas
- `ApplicationCaseException`: excepcion funcional de casos de uso.
- `HandlerGuard`: utilitario de guard clauses para validar precondiciones en handlers.

## Mapping
- `MapsterConfig` registra mapeos de `ElectricityTariff` hacia:
- `GetTariffByIdResponse`
- `GetLatestTariffResponse`
- `GetAllTariffsResponse.TariffItem`
- `CreateTariffResponse`
- `UpdateTariffResponse`

## Inyeccion de dependencias
- `DependencyInjectionApplication.cs` centraliza el registro de servicios de capa `Application` (validadores, handlers y mapeo).

## Patrones aplicados
- CQRS
- Repository + Unit of Work
- Validation per use case
- DTO mapping explicito

Actualizado: 2026-02-17
