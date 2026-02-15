namespace TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Value Object que representa los costos de la tarifa eléctrica
/// </summary>
public record class TariffCosts
(
    decimal? TotalCu,
    decimal? PurchaseCostG,
    decimal? ChargeTransportStnTm,
    decimal? ChargeTransportSdlDm,
    decimal? MarketingMargin,
    decimal? CostLossesPr,
    decimal? RestrictionsRm,
    decimal? Cot,
    decimal? CfmjGfact)
{
    /// <summary>
    /// Calcula la suma de todos los componentes de costo
    /// </summary>
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
}
