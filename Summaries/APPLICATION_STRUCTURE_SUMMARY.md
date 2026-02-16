# 📋 Application Layer - Estructura Implementada

## Estructura de Carpetas

```
Application/
├── Contracts/
│   ├── Persistence/
│   │   └── IUnitOfWork.cs
│   ├── Repositories/
│   │   ├── Generic/
│   │   │   └── IRepository.cs
│   │   ├── ICompanyRepository.cs
│   │   ├── IElectricityTariffRepository.cs
│   │   └── IEtlLogRepository.cs
│   └── Services/
│       └── IEtlService.cs
├── Exceptions/
│   ├── ApplicationCaseException.cs
│   └── HandlerGuard.cs
├── Mapping/
│   └── MapsterConfig.cs
├── UseCases/
│   ├── Commands/
│   │   ├── CreateTariff/
│   │   │   ├── CreateTariffCommand.cs
│   │   │   ├── CreateTariffCommandValidator.cs
│   │   │   ├── CreateTariffCommandHandler.cs
│   │   │   └── CreateTariffResponse.cs
│   │   ├── UpdateTariff/
│   │   │   ├── UpdateTariffCommand.cs
│   │   │   ├── UpdateTariffCommandValidator.cs
│   │   │   ├── UpdateTariffCommandHandler.cs
│   │   │   └── UpdateTariffResponse.cs
│   │   └── DeleteTariff/
│   │       ├── DeleteTariffCommand.cs
│   │       ├── DeleteTariffCommandValidator.cs
│   │       ├── DeleteTariffCommandHandler.cs
│   │       └── DeleteTariffResponse.cs
│   └── Queries/
│       ├── GetAllTariffs/
│       │   ├── GetAllTariffsQuery.cs
│       │   ├── GetAllTariffsQueryValidator.cs
│       │   ├── GetAllTariffsQueryHandler.cs
│       │   └── GetAllTariffsResponse.cs
│       ├── GetLatestTariff/
│       │   ├── GetLatestTariffQuery.cs
│       │   ├── GetLatestTariffQueryHandler.cs
│       │   └── GetLatestTariffResponse.cs
│       ├── GetTariffById/
│       │   ├── GetTariffByIdQuery.cs
│       │   ├── GetTariffByIdQueryValidator.cs
│       │   ├── GetTariffByIdQueryHandler.cs
│       │   └── GetTariffByIdResponse.cs
│       ├── GetTariffByPeriod/
│       │   ├── GetTariffByPeriodQuery.cs
│       │   ├── GetTariffByPeriodQueryValidator.cs
│       │   ├── GetTariffByPeriodQueryHandler.cs
│       │   └── GetTariffByPeriodResponse.cs
│       └── SimulateInvoice/
│           ├── SimulateInvoiceQuery.cs
│           ├── SimulateInvoiceQueryValidator.cs
│           ├── SimulateInvoiceQueryHandler.cs
│           └── SimulateInvoiceResponse.cs
└── DependencyInjectionApplication.cs
```

---

## Componentes Implementados

### 1️⃣ Excepciones
**ApplicationCaseException.cs**
- Excepción personalizada para errores en casos de uso

**HandlerGuard.cs**
- Envoltorio para handlers
- Convierte `DomainRuleException` y excepciones inesperadas en `ApplicationCaseException`

---

### 2️⃣ Contratos

#### **IUnitOfWork**
**Ubicación**: `Contracts/Persistence/IUnitOfWork.cs`
- Coordina repositorios y transacciones
- Repositorios expuestos: `ElectricityTariffs`, `Companies`, `EtlLogs`
- Métodos: `SaveChangesAsync`, `BeginTransactionAsync`, `CommitAsync`, `RollbackAsync`

#### **IRepository<T>**
**Ubicación**: `Contracts/Repositories/Generic/IRepository.cs`
- CRUD genérico + `ExistsAsync`

#### **Repositorios Específicos**
- **IElectricityTariffRepository**: `GetByPeriodAsync(year, period)`, `GetByYearAsync(year)`, `GetLatestAsync()`
- **ICompanyRepository**: `GetByCodeAsync(code)`, `CodeExistsAsync(code)`, `GetAllCompaniesAsync()`
- **IEtlLogRepository**: `GetByStateAsync`, `GetRecentLogsAsync`, `GetLatestAsync`, `GetSuccessRateAsync`

#### **IEtlService**
**Ubicación**: `Contracts/Services/IEtlService.cs`
- Ejecuta el proceso ETL y devuelve `EtlExecutionResult`

---

### 3️⃣ Mapping y DI

#### **MapsterConfig.cs**
- Mapeos `ElectricityTariff → Response` para Create/Update y Queries
- Mapea `TotalCosts` con `GetTotalCosts()`

#### **DependencyInjectionApplication.cs**
- Registra FluentValidation
- Configura Mapster
- Habilita descubrimiento de handlers con WolverineFx

---

### 4️⃣ Casos de Uso (CQRS)

#### ✅ Commands
- **CreateTariff**
  - Valida `CompanyId` y duplicados por `(Year, Period)`
  - Crea `TariffPeriod` y `TariffCosts`
  - Persiste con `IUnitOfWork`

- **UpdateTariff**
  - Solo actualiza costos (no período/operador/empresa)
  - Reemplaza costos con `TariffCosts` nuevo

- **DeleteTariff**
  - Elimina la tarifa por `Id`

#### ✅ Queries
- **GetAllTariffs**
  - Paginación en memoria: `Page` y `PageSize`
  - Ordena por `CreatedAt` desc

- **GetLatestTariff**
  - Usa `GetLatestAsync()`

- **GetTariffById**
  - Busca por `Id`

- **GetTariffByPeriod**
  - Usa `Year` + `Period` (string de Gov.co)

- **SimulateInvoice**
  - Obtiene tarifa + empresa
  - Usa `ElectricityTariff.SimulateInvoice()`
  - Devuelve desglose de costos

---

## Flujo General de Validaciones

```
Request → Validator (FluentValidation) → Handler (WolverineFx)
    └─ HandlerGuard → ApplicationCaseException
```

---

## Características Implementadas

✅ **CQRS Pattern** (Commands / Queries)
✅ **FluentValidation**
✅ **Unit of Work**
✅ **Repository Pattern**
✅ **Mapster Mapping**
✅ **WolverineFx Discovery**
✅ **Exception Handling (HandlerGuard)**

---

*Application Layer - Actualizado: Febrero 2026*
