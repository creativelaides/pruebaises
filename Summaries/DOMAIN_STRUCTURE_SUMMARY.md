# 🏗️ Domain Layer - Estructura Completa

## Arquitectura General

```
Domain/
├── Entities/
│   ├── EntityRoot/
│   │   └── Root.cs
│   ├── Company.cs
│   ├── ElectricityTariff.cs
│   └── EtlLog.cs
├── ValueObjects/
│   ├── TariffPeriod.cs
│   ├── TariffCosts.cs
│   ├── InvoiceComponent.cs
│   └── InvoiceSimulation.cs
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
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime DateUpdated { get; protected set; }
}
```
- Base para todas las entidades
- Guid v7 + auditoría básica

---

## 🏛️ Entidades (3)

### 1️⃣ Company
**Propósito**: Operador distribuidor (operador_de_red de Gov.co)

**Propiedades**:
- `Code` (máx 300)
- Hereda `Id`, `CreatedAt`, `DateUpdated`

**Validaciones**:
- Code requerido
- Code ≤ 300 caracteres

**Método**:
- `UpdateCode(newCode)` con las mismas validaciones

---

### 2️⃣ ElectricityTariff
**Propósito**: Tarifa eléctrica del mercado regulado

**Propiedades**:
- `TariffPeriod Period`
- `TariffCosts Costs`
- `Guid CompanyId`

**Validaciones**:
- Period y Costs no nulos
- CompanyId != Guid.Empty

**Métodos**:
- `UpdateCosts(TariffCosts newCosts)`
- `SimulateInvoice(decimal kwhConsumption)`
- `GetTotalCosts()`

---

### 3️⃣ EtlLog
**Propósito**: Auditoría de ejecuciones ETL

**Propiedades**:
- `ExecutionDate`, `State`, `ProcessedRecords`, `Message`, `DurationSeconds`

**Propiedades computadas**:
- `IsSuccess`, `IsCompleted`, `HasIssues`

---

## 📦 Value Objects (4)

### 1️⃣ TariffPeriod
**Propósito**: Período y contexto de la tarifa

**Propiedades**:
- `Year`, `Period`, `Level`, `TariffOperator`

**Validaciones**:
- Year entre 1900 y `currentYear + 1`
- Period y Level no vacíos, máx 100
- TariffOperator no vacío, máx 300

**Notas**:
- Recibe `currentYear` en el constructor
- Igualdad por valor (Equals / GetHashCode)

---

### 2️⃣ TariffCosts
**Propósito**: 9 componentes de costo

**Validaciones**:
- Ningún componente puede ser negativo

**Método**:
- `CalculateTotal()` suma los 9 componentes

---

### 3️⃣ InvoiceComponent
**Propósito**: Componente individual de factura simulada

**Validaciones**:
- Name requerido, máx 100
- Explanation requerido, máx 500
- Value no negativo

---

### 4️⃣ InvoiceSimulation
**Propósito**: Resultado de simulación de factura

**Contenido**:
- Consumo, costos parciales, total y lista de componentes

---

## 🔢 Enums (1)

### **EtlState**
```csharp
public enum EtlState
{
    Running = 1,
    Success = 2,
    Failed = 3,
    Cancelled = 4
}
```

---

## ❌ Excepciones (1)

### **DomainRuleException**
- Excepción para violaciones de reglas de negocio

---

## 📊 Diagrama de Relaciones

```
Root (abstract)
  ├── Company
  ├── ElectricityTariff
  │   ├── Usa: TariffPeriod (VO)
  │   ├── Usa: TariffCosts (VO)
  │   └── Usa: InvoiceSimulation (VO)
  └── EtlLog
      └── Usa: EtlState (Enum)

ValueObjects:
  - TariffPeriod
  - TariffCosts
  - InvoiceComponent
  - InvoiceSimulation
```

---

## 🎯 Patrones Aplicados

✅ **Entity Root**
✅ **Value Object**
✅ **Exception Pattern**
✅ **Auditoría en Root**
✅ **Simulación de Factura**

---

*Domain Layer - Actualizado: Febrero 2026*
