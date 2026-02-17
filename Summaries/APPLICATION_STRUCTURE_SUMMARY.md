# 💼 Application Layer - Estructura Completa

## Arquitectura General

```
Application/
├── Contracts/
│   ├── Identity/
│   │   └── IAppUserService.cs
│   ├── Persistence/
│   │   └── IUnitOfWork.cs
│   ├── Repositories/
│   │   ├── Generic/IRepository.cs
│   │   ├── ICompanyRepository.cs
│   │   ├── IElectricityTariffRepository.cs
│   │   └── IEtlLogRepository.cs
│   └── Services/
│       ├── IEtlService.cs
│       └── IEmailService.cs
├── Exceptions/
│   └── ApplicationException.cs
├── Mapping/
│   └── MappingProfile.cs
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

---

## 📌 Contratos (Contracts)

**Repositorios**
- `IRepository<T>`: contrato genérico para CRUD.
- `IElectricityTariffRepository`, `ICompanyRepository`, `IEtlLogRepository`.

**Persistencia**
- `IUnitOfWork`: agrupa repositorios y `SaveChangesAsync`.

**Servicios**
- `IEtlService`: ejecutar ETL y leer logs.
- `IEmailService`: envío de correo (infraestructura).

**Identity**
- `IAppUserService`: obtener el `UserId` actual para auditoría.

---

## ✅ Use Cases (CQRS)

**Commands (escritura)**
- `CreateTariffCommand` + `Validator` + `Handler` + `Response`
- `UpdateTariffCommand` + `Validator` + `Handler` + `Response`
- `DeleteTariffCommand` + `Validator` + `Handler` + `Response`

**Queries (lectura)**
- `GetAllTariffsQuery` + `Handler` + `Response`
- `GetLatestTariffQuery` + `Handler` + `Response`
- `GetTariffByIdQuery` + `Handler` + `Response`
- `GetTariffByPeriodQuery` + `Handler` + `Response`
- `SimulateInvoiceQuery` + `Handler` + `Response`

---

## 🧩 Mapping

**MappingProfile**
- Define proyecciones de Domain → DTOs de respuesta.

---

## ⚠️ Exceptions

**ApplicationException**
- Representa errores funcionales propios de la capa Application.

---

## 🎯 Patrones Aplicados

✅ **CQRS (Commands/Queries)**  
✅ **Repository + UnitOfWork**  
✅ **Validation por caso de uso**  
✅ **Mapeo explícito de respuestas**

---

*Application Layer - Actualizado: Febrero 2026*
