# Domain Layer - Estructura Actualizada

## Vista general
La capa `Domain` concentra las reglas de negocio puras del sistema de tarifas electricas. No depende de infraestructura y modela entidades, objetos de valor y excepciones de dominio.

```text
Backend/src/Core/TarifasElectricas.Domain/
├── Entities/
│   ├── Root/
│   │   ├── RootEntity.cs
│   │   └── AuditableEntity.cs
│   ├── Company.cs
│   ├── ElectricityTariff.cs
│   └── EtlLog.cs
├── ValueObjects/
│   ├── TariffPeriod.cs
│   ├── TariffCosts.cs
│   ├── InvoiceComponent.cs
│   └── InvoiceSimulation.cs
├── Enums/EtlState.cs
└── Exceptions/DomainRuleException.cs
```

## Entidades base
- `RootEntity`: id, fecha de creacion y fecha de actualizacion.
- `AuditableEntity`: agrega campos de auditoria de usuario (`CreatedBy`, `UpdatedBy`).

## Entidades del dominio
- `Company`: comercializadora/operador, con validaciones de codigo y metodo de actualizacion.
- `ElectricityTariff`: agregado principal; relaciona `TariffPeriod`, `TariffCosts` y `CompanyId`; expone `UpdateCosts`, `GetTotalCosts` y simulacion de factura.
- `EtlLog`: registro de ejecuciones ETL con estado, fecha, duracion, procesados y mensaje; incluye propiedades derivadas para estado de ejecucion.

## Value objects
- `TariffPeriod`: anio, periodo, nivel y operador de red con validaciones de formato/rango.
- `TariffCosts`: componentes economicos de la tarifa y calculo de total.
- `InvoiceComponent`: detalle de cada componente en una simulacion.
- `InvoiceSimulation`: resultado agregado de simulacion de factura.

## Enumeraciones
- `EtlState`: `Running`, `Success`, `Failed`, `Cancelled`.

## Excepciones de dominio
- `DomainRuleException`: se lanza cuando se incumplen invariantes o reglas de negocio.

## Relaciones principales
- `ElectricityTariff` usa `TariffPeriod` y `TariffCosts`.
- `ElectricityTariff` produce `InvoiceSimulation` compuesta por `InvoiceComponent`.
- `EtlLog` usa `EtlState`.
- `Company`, `ElectricityTariff` y `EtlLog` heredan de `AuditableEntity`.

## Patrones aplicados
- Entity + Aggregate behavior
- Value Object
- Domain exception
- Auditable base entities

Actualizado: 2026-02-17
