# 📋 Application Layer - Estructura Implementada

## Estructura de Carpetas

```
Application/
├── Exceptions/
│   └── ApplicationCaseException.cs
├── Contracts/
│   ├── Persistence/
│   │   └── IUnitOfWork.cs
│   └── Repositories/
│       ├── Base/
│       │   └── IRepository.cs (genérica)
│       ├── IElectricityTariffRepository.cs
│       ├── IEducationComponentRepository.cs
│       └── IEtlLogRepository.cs
└── UseCases/
    ├── Commands/
    │   └── CreateTariff/
    │       ├── CreateTariffCommand.cs
    │       ├── CreateTariffDto.cs
    │       ├── CreateTariffCommandValidator.cs
    │       ├── CreateTariffCommandHandler.cs
    │       └── CreateTariffResponse.cs
    └── Queries/
        └── (próximamente)
```

---

## Componentes Implementados

### 1️⃣ Excepciones
**ApplicationCaseException.cs**
- Excepción personalizada para errores en casos de uso
- Hereda de Exception
- Constructor con message e innerException

---

### 2️⃣ Contratos (Persistence)

#### **IUnitOfWork**
**Ubicación**: `Contracts/Persistence/IUnitOfWork.cs`

```csharp
public interface IUnitOfWork : IDisposable
{
    IElectricityTariffRepository ElectricityTariffs { get; }
    IEducationComponentRepository EducationComponents { get; }
    IEtlLogRepository EtlLogs { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
```
- Gestiona transacciones
- Coordina múltiples repositorios
- Patrón Unit of Work

#### **IRepository<T>** (Genérica - Subcarpeta Generic)
**Ubicación**: `Contracts/Persistence/Repositories/Generic/IRepository.cs`

```csharp
public interface IRepository<T> where T : Root
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(Guid id);
}
```
- Operaciones CRUD básicas
- Constraint: solo entidades que heredan de Root
- Base para todos los repositorios específicos

#### **IElectricityTariffRepository** (Específica)
**Ubicación**: `Contracts/Persistence/Repositories/IElectricityTariffRepository.cs`

```csharp
public interface IElectricityTariffRepository : IRepository<ElectricityTariff>
{
    Task<ElectricityTariff?> GetByPeriodAsync(int year, int month);
    Task<IEnumerable<ElectricityTariff>> GetByYearAsync(int year);
    Task<ElectricityTariff?> GetLatestAsync();
}
```
- Hereda operaciones genéricas de IRepository<T>
- Métodos específicos de negocio
- Al mismo nivel que IUnitOfWork en la carpeta Persistence

#### **IEducationComponentRepository** (Específica)
**Ubicación**: `Contracts/Persistence/Repositories/IEducationComponentRepository.cs`

```csharp
public interface IEducationComponentRepository : IRepository<EducationComponent>
{
    Task<EducationComponent?> GetByCodeAsync(string code);
    Task<IEnumerable<EducationComponent>> GetByOrderAsync(int order);
    Task<bool> CodeExistsAsync(string code);
}
```
- Hereda operaciones genéricas de IRepository<T>
- Al mismo nivel que IUnitOfWork en la carpeta Persistence

#### **IEtlLogRepository** (Específica)
**Ubicación**: `Contracts/Persistence/Repositories/IEtlLogRepository.cs`

```csharp
public interface IEtlLogRepository : IRepository<EtlLog>
{
    Task<IEnumerable<EtlLog>> GetByStateAsync(EtlState state);
    Task<IEnumerable<EtlLog>> GetRecentLogsAsync(int days);
    Task<EtlLog?> GetLatestAsync();
    Task<decimal> GetSuccessRateAsync(int days);
}
```
- Hereda operaciones genéricas de IRepository<T>
- Al mismo nivel que IUnitOfWork en la carpeta Persistence

---

### 3️⃣ Caso de Uso: CreateTariff

#### **CreateTariffDto**
```csharp
public class CreateTariffDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Period { get; set; }
    public string? Level { get; set; }
    public string? Operator { get; set; }
    
    public decimal? TotalCu { get; set; }
    public decimal? PurchaseCostG { get; set; }
    // ... 7 propiedades más de costos
}
```
- Transporta datos desde la entrada (Controller)
- No incluye lógica de negocio

#### **CreateTariffCommand**
```csharp
public class CreateTariffCommand
{
    public CreateTariffCommand(CreateTariffDto tariffData)
    {
        TariffData = tariffData ?? throw new ArgumentNullException(nameof(tariffData));
    }

    public CreateTariffDto TariffData { get; }
}
```
- Encapsula el DTO
- Garantiza que TariffData no es null

#### **CreateTariffCommandValidator**
```csharp
public class CreateTariffCommandValidator : AbstractValidator<CreateTariffCommand>
{
    public CreateTariffCommandValidator()
    {
        RuleFor(x => x.TariffData).NotNull();
        RuleFor(x => x.TariffData.Year).GreaterThanOrEqualTo(1900);
        RuleFor(x => x.TariffData.Month).GreaterThanOrEqualTo(1).LessThanOrEqualTo(12);
        // ... más validaciones
    }
}
```
- FluentValidation
- Validaciones de aplicación (no de dominio)
- Se ejecuta antes del handler

#### **CreateTariffCommandHandler**
```csharp
public class CreateTariffCommandHandler
{
    public async Task<CreateTariffResponse> HandleAsync(CreateTariffCommand command)
    {
        // 1. Verificar si existe
        // 2. Crear Value Objects
        // 3. Crear Entity
        // 4. Persistir
        // 5. Retornar response
    }
}
```
- Lógica del caso de uso
- Orquesta el flujo
- Maneja excepciones (Domain → Application)

#### **CreateTariffResponse**
```csharp
public class CreateTariffResponse
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Period { get; set; }
    public string? Level { get; set; }
    public string? Operator { get; set; }
    public decimal TotalCosts { get; set; }
    public DateTime CreatedAt { get; set; }
}
```
- Retorna datos al cliente
- DTO de salida

---

## Flujo de CreateTariffCommand

```
Controller
    ↓
CreateTariffDto (entrada)
    ↓
CreateTariffCommand (encapsula)
    ↓
CreateTariffCommandValidator (valida)
    ↓
CreateTariffCommandHandler (ejecuta)
    │
    ├─ Verifica existencia (IUnitOfWork)
    ├─ Crea Value Objects (pueden lanzar DomainRuleException)
    ├─ Crea Entity (ElectricityTariff)
    ├─ Persistencia (IUnitOfWork.SaveChangesAsync)
    └─ Maneja excepciones
    ↓
CreateTariffResponse (salida)
    ↓
Controller → JSON
```

---

## Manejo de Excepciones

```
DomainRuleException (Domain Layer)
    ↓
    (Capturada en Handler)
    ↓
ApplicationCaseException (Application Layer)
    ↓
    (Controlador de Middleware)
    ↓
HTTP Response (400 Bad Request)
```

---

## Próximos Pasos

1. Crear más Commands (UpdateTariff, DeleteTariff)
2. Crear Queries (GetTariffById, GetAllTariffs)
3. Implementar Repositories (EF Core)
4. Registrar en DI (Program.cs)
5. Crear Controllers

---

## Características Implementadas

✅ **CQRS Pattern** (Commands y Queries separados)
✅ **FluentValidation** (Validaciones de aplicación)
✅ **Unit of Work** (Patrón transacional)
✅ **Repository Pattern** (Abstracción de persistencia)
✅ **DTO Pattern** (Transferencia de datos)
✅ **Exception Handling** (Gestión de excepciones por capas)
✅ **Type Safety** (Interfaz genérica IRepository<T>)

---

## 📂 Estructura Detallada de Contracts

```
Contracts/
├── Persistence/
│   └── IUnitOfWork.cs
└── Repositories/
    ├── Base/
    │   └── IRepository.cs (genérica)
    ├── IElectricityTariffRepository.cs
    ├── IEducationComponentRepository.cs
    └── IEtlLogRepository.cs
```

### Jerarquía de Interfaces

```
IRepository<T>                              (Base genérica - en carpeta Base/)
    ↑
    ├─ IElectricityTariffRepository          (Hereda + métodos específicos)
    ├─ IEducationComponentRepository         (Hereda + métodos específicos)
    └─ IEtlLogRepository                     (Hereda + métodos específicos)

IUnitOfWork                                 (en Persistence/)
    ├─ IElectricityTariffRepository
    ├─ IEducationComponentRepository
    └─ IEtlLogRepository
```

### Operaciones por Interfaz

| Interfaz | Ubicación | Operaciones |
|----------|-----------|------------|
| **IRepository<T>** | `Contracts/Repositories/Base/` | CRUD genérico |
| **IElectricityTariffRepository** | `Contracts/Repositories/` | CRUD + GetByPeriod, GetByYear, GetLatest |
| **IEducationComponentRepository** | `Contracts/Repositories/` | CRUD + GetByCode, GetByOrder, CodeExists |
| **IEtlLogRepository** | `Contracts/Repositories/` | CRUD + GetByState, GetRecentLogs, GetLatest, GetSuccessRate |
| **IUnitOfWork** | `Contracts/Persistence/` | Transacciones + Acceso a repositorios |

---
