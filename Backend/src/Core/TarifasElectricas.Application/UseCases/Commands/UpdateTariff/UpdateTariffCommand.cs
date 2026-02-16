namespace TarifasElectricas.Application.UseCases.Commands.UpdateTariff;

/// <summary>
/// Comando para actualizar los componentes de costo de una tarifa existente
/// 
/// Nota: Solo permite actualizar costos, NO el período/operador/empresa
/// (los datos de Gov.co son inmutables una vez creados)
/// 
/// Parámetros:
/// - Id: ID de la tarifa a actualizar
/// - 9 componentes de costo nuevos
/// </summary>
public record UpdateTariffCommand(
    Guid Id,
    decimal? TotalCu,
    decimal? PurchaseCostG,
    decimal? ChargeTransportStnTm,
    decimal? ChargeTransportSdlDm,
    decimal? MarketingMargin,
    decimal? CostLossesPr,
    decimal? RestrictionsRm,
    decimal? Cot,
    decimal? CfmjGfact);