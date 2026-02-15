namespace TarifasElectricas.Application.UseCases.Commands.UpdateTariff;

/// <summary>
/// Comando para actualizar una tarifa eléctrica existente.
/// Record - Inmutable.
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
