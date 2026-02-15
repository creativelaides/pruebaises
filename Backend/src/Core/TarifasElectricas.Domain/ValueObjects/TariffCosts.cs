using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Domain.ValueObjects;


/// <summary>
/// Value Object que representa los costos de la tarifa eléctrica
/// </summary>
public record TariffCosts
{
    public decimal? TotalCu { get; init; }
    public decimal? PurchaseCostG { get; init; }
    public decimal? ChargeTransportStnTm { get; init; }
    public decimal? ChargeTransportSdlDm { get; init; }
    public decimal? MarketingMargin { get; init; }
    public decimal? CostLossesPr { get; init; }
    public decimal? RestrictionsRm { get; init; }
    public decimal? Cot { get; init; }
    public decimal? CfmjGfact { get; init; }
    public TariffCosts(
        decimal? totalCu,
        decimal? purchaseCostG,
        decimal? chargeTransportStnTm,
        decimal? chargeTransportSdlDm,
        decimal? marketingMargin,
        decimal? costLossesPr,
        decimal? restrictionsRm,
        decimal? cot,
        decimal? cfmjGfact)
    {
        if (totalCu.HasValue && totalCu < 0)
            throw new DomainRuleException($"TotalCu no puede ser negativo");

        if (purchaseCostG.HasValue && purchaseCostG < 0)
            throw new DomainRuleException($"PurchaseCostG no puede ser negativo");

        if (chargeTransportStnTm.HasValue && chargeTransportStnTm < 0)
            throw new DomainRuleException($"ChargeTransportStnTm no puede ser negativo");

        if (chargeTransportSdlDm.HasValue && chargeTransportSdlDm < 0)
            throw new DomainRuleException($"ChargeTransportSdlDm no puede ser negativo");

        if (marketingMargin.HasValue && marketingMargin < 0)
            throw new DomainRuleException($"MarketingMargin no puede ser negativo");

        if (costLossesPr.HasValue && costLossesPr < 0)
            throw new DomainRuleException($"CostLossesPr no puede ser negativo");

        if (restrictionsRm.HasValue && restrictionsRm < 0)
            throw new DomainRuleException($"RestrictionsRm no puede ser negativo");

        if (cot.HasValue && cot < 0)
            throw new DomainRuleException($"Cot no puede ser negativo");

        if (cfmjGfact.HasValue && cfmjGfact < 0)
            throw new DomainRuleException($"CfmjGfact no puede ser negativo");

        TotalCu = totalCu;
        PurchaseCostG = purchaseCostG;
        ChargeTransportStnTm = chargeTransportStnTm;
        ChargeTransportSdlDm = chargeTransportSdlDm;
        MarketingMargin = marketingMargin;
        CostLossesPr = costLossesPr;
        RestrictionsRm = restrictionsRm;
        Cot = cot;
        CfmjGfact = cfmjGfact;
    }

    /// <summary>
    /// Calcula el total de costos
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