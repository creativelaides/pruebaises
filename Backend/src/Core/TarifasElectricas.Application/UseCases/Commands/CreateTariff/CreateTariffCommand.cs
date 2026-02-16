namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Comando para crear una nueva tarifa de energía eléctrica
/// 
/// Datos requeridos:
/// - Período: Año y período específico (ej: 2025, "Enero")
/// - Nivel: Categoría del cliente (ej: "Nivel 1 (Propiedad OR)")
/// - Operador: Empresa distribuidora (ej: "ENEL Bogotá - Cundinamarca")
/// - CompanyId: ID de la entidad Company (FK)
/// - Componentes de costo: 9 valores numéricos de Gov.co
/// 
/// Validaciones:
/// - Year: 1900 - año actual + 1
/// - CompanyId: No puede ser Guid.Empty
/// - Todos los datos requeridos
/// </summary>
public record CreateTariffCommand(
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal? TotalCu,
    decimal? PurchaseCostG,
    decimal? ChargeTransportStnTm,
    decimal? ChargeTransportSdlDm,
    decimal? MarketingMargin,
    decimal? CostLossesPr,
    decimal? RestrictionsRm,
    decimal? Cot,
    decimal? CfmjGfact);