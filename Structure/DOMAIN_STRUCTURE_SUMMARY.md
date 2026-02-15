# 🏗️ Domain Layer - Estructura Completa

## Arquitectura General

```
Domain/
├── Entities/
│   ├── EntityRoot/
│   │   └── Root.cs
│   ├── EducationComponent.cs
│   ├── ElectricityTariff.cs
│   └── EtlLog.cs
├── ValueObjects/
│   ├── TariffPeriod.cs
│   └── TariffCosts.cs
├── Enums/
│   └── EtlState.cs
└── Exceptions/
    └── DomainRuleException.cs
```

---

## 📌 Entity Root (Base)

### **Root.cs**
```csharp
public abstract class Root
{
    public Guid Id { get; set; }
}
```
- Clase base abstracta para todas las entidades
- Id tipo Guid como clave primaria
- Garantiza identidad única en el dominio

---

## 🏛️ Entidades (3)

### 1️⃣ EducationComponent
**Propósito**: Componente educativo del sistema ETL

**Propiedades**:
```csharp
public Guid Id { get; set; }                    // Heredado de Root
public string? Code { get; set; }               // Requerido
public string? Name { get; set; }               // Requerido
public string? SimpleDescription { get; set; }
public string? Analogy { get; set; }
public string? Icon { get; set; }
public string? Color { get; set; }
public int? Order { get; set; }
```

**Constructor**:
```csharp
private EducationComponent() { }  // Para EF Core

public EducationComponent(
    string? code,
    string? name,
    string? simpleDescription,
    string? analogy,
    string? icon,
    string? color,
    int? order)
{
    // Validaciones:
    // - Code no puede estar vacío
    // - Name es requerido
    
    Id = Guid.CreateVersion7();
    Code = code;
    Name = name;
    SimpleDescription = simpleDescription;
    Analogy = analogy;
    Icon = icon;
    Color = color;
    Order = order;
}
```

**Validaciones**:
- ✅ Código no vacío
- ✅ Nombre requerido

---

### 2️⃣ ElectricityTariff
**Propósito**: Tarifa eléctrica con período y costos

**Propiedades**:
```csharp
public Guid Id { get; set; }                    // Heredado de Root
public TariffPeriod Period { get; set; }        // Value Object
public TariffCosts Costs { get; set; }          // Value Object
public DateTime DateUpdated { get; set; }
public DateTime CreatedAt { get; set; }
```

**Constructor**:
```csharp
private ElectricityTariff() { }  // Para EF Core

public ElectricityTariff(TariffPeriod period, TariffCosts costs)
{
    Id = Guid.CreateVersion7();
    Period = period;
    Costs = costs;
    CreatedAt = DateTime.UtcNow;
    DateUpdated = DateTime.UtcNow;
}
```

**Métodos**:
```csharp
public void UpdateCosts(TariffCosts newCosts)
{
    Costs = newCosts;
    DateUpdated = DateTime.UtcNow;
}

public decimal GetTotalCosts() => Costs.CalculateTotalComponents();
```

**Validaciones** (en Value Objects):
- ✅ Año entre 1900 y año actual + 1
- ✅ Mes entre 1 y 12
- ✅ Costos no negativos

---

### 3️⃣ EtlLog
**Propósito**: Auditoría de ejecución del proceso ETL

**Propiedades**:
```csharp
public Guid Id { get; set; }                    // Heredado de Root
public DateTime ExecutionDate { get; set; }
public EtlState State { get; set; }             // Enum
public int? ProcessedRecords { get; set; }
public string? Message { get; set; }
public decimal? DurationSeconds { get; set; }
```

**Constructor**:
```csharp
private EtlLog() { }  // Para EF Core

public EtlLog(
    DateTime executionDate,
    EtlState state,
    int? processedRecords = null,
    string? message = null,
    decimal? durationSeconds = null)
{
    Id = Guid.CreateVersion7();
    ExecutionDate = executionDate;
    State = state;
    ProcessedRecords = processedRecords;
    Message = message;
    DurationSeconds = durationSeconds;
}
```

**Propiedades Computadas**:
```csharp
public bool IsSuccess => State == EtlState.Success;
public bool IsCompleted => State != EtlState.Running;
public bool HasIssues => State == EtlState.Failed || State == EtlState.Cancelled;
```

**Validaciones**:
- ✅ Sin validaciones adicionales (simple)

---

## 📦 Value Objects (2)

### 1️⃣ TariffPeriod
**Propósito**: Encapsular información del período y clasificación de tarifa

**Tipo**: Record (inmutable)

**Propiedades**:
```csharp
public record TariffPeriod(
    int Year,              // 1900 a año actual + 1
    int Month,             // 1 a 12
    string? Period,        // No vacío
    string? Level,         // No vacío
    string? Operator)      // No vacío
```

**Constructor con Validaciones**:
```csharp
public TariffPeriod(int year, int month, string? period, string? level, string? @operator)
    : this(year, month, period, level, @operator)
{
    if (year < 1900 || year > DateTime.UtcNow.Year + 1)
        throw new DomainRuleException($"Año inválido: {year}");

    if (month < 1 || month > 12)
        throw new DomainRuleException($"Mes inválido: {month}");
}
```

**Validaciones**:
- ✅ Año válido (1900 a año actual + 1)
- ✅ Mes válido (1 a 12)

**Características**:
- Igualdad automática (records)
- GetHashCode() automático
- ToString() automático
- Inmutabilidad por defecto

---

### 2️⃣ TariffCosts
**Propósito**: Encapsular todos los componentes de costo de una tarifa

**Tipo**: Record (inmutable)

**Propiedades**:
```csharp
public record TariffCosts(
    decimal? TotalCu,                   // Costo total
    decimal? PurchaseCostG,             // Costo de compra
    decimal? ChargeTransportStnTm,      // Cargo transporte STN/TM
    decimal? ChargeTransportSdlDm,      // Cargo transporte SDL/DM
    decimal? MarketingMargin,           // Margen marketing
    decimal? CostLossesPr,              // Costo pérdidas
    decimal? RestrictionsRm,            // Restricciones
    decimal? Cot,                       // COT
    decimal? CfmjGfact)                 // CFMJ G-Factor
```

**Constructor con Validaciones**:
```csharp
public TariffCosts(...) : this(...)
{
    // Valida que ningún valor sea negativo
    // Lanza DomainRuleException si alguno es < 0
}
```

**Métodos**:
```csharp
public decimal CalculateTotalComponents() =>
    (TotalCu ?? 0) +
    (PurchaseCostG ?? 0) +
    (ChargeTransportStnTm ?? 0) +
    (ChargeTransportSdlDm ?? 0) +
    (MarketingMargin ?? 0) +
    (CostLossesPr ?? 0) +
    (RestrictionsRm ?? 0) +
    (Cot ?? 0) +
    (CfmjGfact ?? 0);
```

**Validaciones**:
- ✅ Ningún componente puede ser negativo
- ✅ Null permitido (usa ?? 0 en suma)

**Características**:
- Suma todos los componentes
- Maneja valores nulos correctamente
- Igualdad automática

---

## 🔢 Enums (1)

### **EtlState**
```csharp
public enum EtlState
{
    Running = 1,      // ⚙️ En ejecución
    Success = 2,      // ✅ Exitoso
    Failed = 3,       // ❌ Fallido
    Cancelled = 4     // ⚠️ Cancelado
}
```

**Propósito**: Representar los 4 estados principales de un proceso ETL

**Transiciones**:
- Running → Success, Failed, Cancelled (terminal)
- Success → (Terminal)
- Failed → (Terminal)
- Cancelled → (Terminal)

---

## ❌ Excepciones (1)

### **DomainRuleException**
```csharp
public class DomainRuleException : Exception
{
    public DomainRuleException(string message) : base(message) { }
    
    public DomainRuleException(string message, Exception innerException) 
        : base(message, innerException) { }
}
```

**Propósito**: Lanzar excepciones cuando se viola una regla de negocio del dominio

**Cuándo Usar**:
- ✅ Validaciones en constructores de entities y VOs
- ✅ Violaciones de invariantes
- ✅ Condiciones de negocio inválidas

**Cuándo NO Usar**:
- ❌ Errores de BD
- ❌ Errores de red
- ❌ Errores de serialización

---

## 📊 Diagrama de Relaciones

```
Root (abstract)
  ├── EducationComponent
  ├── ElectricityTariff
  │   ├── Usa: TariffPeriod (VO)
  │   └── Usa: TariffCosts (VO)
  └── EtlLog
      └── Usa: EtlState (Enum)

ValueObjects:
  - TariffPeriod (record)
  - TariffCosts (record)

Enums:
  - EtlState (4 valores)

Exceptions:
  - DomainRuleException
```

---

## 🎯 Patrones Aplicados

### 1. **Entity Root Pattern**
- Base abstracta con Guid Id
- Garantiza identidad única

### 2. **Value Object Pattern**
- Records para inmutabilidad
- Validaciones en constructor
- Igualdad por valor

### 3. **Constructor Pattern**
- Constructor privado vacío (EF Core)
- Constructor público con Guid.CreateVersion7()
- Validaciones en constructor público

### 4. **Anemic Domain vs Rich Domain**
- **Rich**: ElectricityTariff.GetTotalCosts()
- **Simple**: Campos públicos con setters

### 5. **Exception Pattern**
- DomainRuleException para errores de negocio
- Lanzadas en constructores

---

## 🔍 Validaciones por Capa

### **Domain Layer** (Validaciones de Negocio)
- ✅ Año entre 1900 y actual + 1 (TariffPeriod)
- ✅ Mes entre 1 y 12 (TariffPeriod)
- ✅ Costos no negativos (TariffCosts)
- ✅ Code y Name no vacíos (EducationComponent)

### **Application Layer** (Validaciones de Aplicación)
- ✅ Lógica de casos de uso
- ✅ Verificación de duplicados
- ✅ Autorización/Permisos

### **API Layer** (Validaciones de Entrada)
- ✅ Formato de datos
- ✅ Rango de valores
- ✅ Campos requeridos

---

## 📈 Características del Domain

✅ **Encapsulación**: Value Objects con validaciones  
✅ **Inmutabilidad**: Records para VOs  
✅ **Type Safety**: Enums en lugar de strings  
✅ **Rich Logic**: Métodos en entities (GetTotalCosts)  
✅ **Exception Handling**: DomainRuleException  
✅ **Guid as PK**: Guid.CreateVersion7()  
✅ **Timestamp Tracking**: CreatedAt, DateUpdated  

---

## 🧪 Cobertura de Tests

| Entidad/VO | Tests | Casos |
|-----------|-------|-------|
| TariffPeriod | 3 | Creación, Año inválido, Mes inválido |
| TariffCosts | 3 | Creación, Negativo, Cálculo |
| ElectricityTariff | 3 | Creación, Update, GetTotal |
| EducationComponent | 3 | Creación, Code null, Name null |
| EtlLog | 3 | Creación, IsSuccess, HasIssues |
| **TOTAL** | **15** | - |

---

## 🚀 Próximos Pasos (Infrastructure)

1. **EF Core Configurations**
   - EntityTypeConfiguration para cada entidad
   - Owned Types para Value Objects

2. **Migrations**
   - Crear base de datos
   - Tablas y relaciones

3. **Repositories**
   - Implementar IRepository<T>
   - Implementar repositorios específicos

4. **UnitOfWork**
   - Implementación con EF Core
   - Gestión de transacciones

---

## 📝 Nomenclatura y Convenciones

| Concepto | Nomenclatura | Ejemplo |
|----------|-------------|---------|
| Entity | PascalCase | ElectricityTariff |
| Property | PascalCase | TotalCu |
| ValueObject | PascalCase | TariffPeriod |
| Enum | PascalCase | EtlState |
| Exception | *Exception | DomainRuleException |
| Private Field | _camelCase | _value |
| Method | PascalCase | GetTotalCosts() |

---

## 💡 Principios SOLID Aplicados

| Principio | Implementación |
|-----------|----------------|
| **S**ingle Responsibility | Cada VO/Entity tiene un propósito único |
| **O**pen/Closed | Root abstracta, extensible |
| **L**iskov Substitution | EducationComponent es Root |
| **I**nterface Segregation | (Aplicado en Application Layer) |
| **D**ependency Inversion | (Aplicado en Application Layer) |

---

## 📦 Tipos de Datos

| Propiedad | Tipo | Nullable | Por Qué |
|-----------|------|----------|--------|
| Id | Guid | ❌ | Identidad única |
| Year | int | ❌ | Requerido |
| Month | int | ❌ | Requerido |
| Period | string | ✅ | Opcional |
| TotalCu | decimal? | ✅ | Componente opcional |
| CreatedAt | DateTime | ❌ | Auditoría |
| State | EtlState | ❌ | Requerido |

---

*Domain Layer - Estructura Completa: Febrero 2026*
