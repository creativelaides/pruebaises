// Domain/ValueObjects/TariffCosts.cs

namespace TarifasElectricas.Domain.ValueObjects;

using TarifasElectricas.Domain.Exceptions;

/// <summary>
/// Value Object que encapsula los 9 componentes de costo de energía eléctrica.
/// 
/// Estos son los datos crudos obtenidos directamente de Gov.co.
/// Cada componente representa una parte del costo final pagado por el usuario.
/// 
/// Mapeo de campos Gov.co:
/// 1. cu_total → TotalCu
/// 2. costo_compra_gmj → PurchaseCostG
/// 3. cargo_transporte_stn_tm → ChargeTransportStnTm
/// 4. cargo_transporte_sdl_dm → ChargeTransportSdlDm
/// 5. margen_comercializacion_cvm → MarketingMargin
/// 6. costo_gt_perdidas_prm → CostLossesPr
/// 7. restricciones_rm → RestrictionsRm
/// 8. cot → Cot
/// 9. cfmj_gfact → CfmjGfact
/// </summary>
public class TariffCosts
{
    /// <summary>
    /// CU Total - Costo unitario total por kWh
    /// Viene de: cu_total (Gov.co)
    /// </summary>
    public decimal? TotalCu { get; init; }

    /// <summary>
    /// Costo de Compra de Energía
    /// Viene de: costo_compra_gmj (Gov.co)
    /// </summary>
    public decimal? PurchaseCostG { get; init; }

    /// <summary>
    /// Cargo Transporte STN (Sistema Transmisión Nacional)
    /// Viene de: cargo_transporte_stn_tm (Gov.co)
    /// </summary>
    public decimal? ChargeTransportStnTm { get; init; }

    /// <summary>
    /// Cargo Transporte SDL (Sistema Distribución Local)
    /// Viene de: cargo_transporte_sdl_dm (Gov.co)
    /// </summary>
    public decimal? ChargeTransportSdlDm { get; init; }

    /// <summary>
    /// Margen de Comercialización
    /// Viene de: margen_comercializacion_cvm (Gov.co)
    /// </summary>
    public decimal? MarketingMargin { get; init; }

    /// <summary>
    /// Costo G.T. Pérdidas (Generación, Transmisión, Distribución)
    /// Viene de: costo_gt_perdidas_prm (Gov.co)
    /// </summary>
    public decimal? CostLossesPr { get; init; }

    /// <summary>
    /// Restricciones
    /// Viene de: restricciones_rm (Gov.co)
    /// </summary>
    public decimal? RestrictionsRm { get; init; }

    /// <summary>
    /// COT (Costo de Operación y Transporte)
    /// Viene de: cot (Gov.co)
    /// </summary>
    public decimal? Cot { get; init; }

    /// <summary>
    /// CFMJ G-Factor (Factor de Carga)
    /// Viene de: cfmj_gfact (Gov.co)
    /// </summary>
    public decimal? CfmjGfact { get; init; }

    /// <summary>
    /// Constructor con validaciones
    /// </summary>
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
        ValidateCosts(totalCu, purchaseCostG, chargeTransportStnTm, chargeTransportSdlDm,
                      marketingMargin, costLossesPr, restrictionsRm, cot, cfmjGfact);

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
    /// Validaciones: ningún componente puede ser negativo
    /// </summary>
    private static void ValidateCosts(
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
        var costs = new[] { totalCu, purchaseCostG, chargeTransportStnTm,
                           chargeTransportSdlDm, marketingMargin, costLossesPr,
                           restrictionsRm, cot, cfmjGfact };

        foreach (var cost in costs.Where(c => c.HasValue && c < 0))
            throw new DomainRuleException("Ningún componente de costo puede ser negativo");
    }

    /// <summary>
    /// Calcula el costo total sumando todos los componentes
    /// Útil para factura simulada
    /// </summary>
    public decimal CalculateTotal() =>
        (TotalCu ?? 0) +
        (PurchaseCostG ?? 0) +
        (ChargeTransportStnTm ?? 0) +
        (ChargeTransportSdlDm ?? 0) +
        (MarketingMargin ?? 0) +
        (CostLossesPr ?? 0) +
        (RestrictionsRm ?? 0) +
        (Cot ?? 0) +
        (CfmjGfact ?? 0);

    /// <summary>
    /// Igualdad por valor
    /// </summary>
    public override bool Equals(object? obj) =>
        obj is TariffCosts other &&
        TotalCu == other.TotalCu &&
        PurchaseCostG == other.PurchaseCostG &&
        ChargeTransportStnTm == other.ChargeTransportStnTm &&
        ChargeTransportSdlDm == other.ChargeTransportSdlDm &&
        MarketingMargin == other.MarketingMargin &&
        CostLossesPr == other.CostLossesPr &&
        RestrictionsRm == other.RestrictionsRm &&
        Cot == other.Cot &&
        CfmjGfact == other.CfmjGfact;

    /// <summary>
    /// Hash code basado en algunas de las propiedades
    /// </summary>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(TotalCu);
        hash.Add(PurchaseCostG);
        hash.Add(ChargeTransportStnTm);
        hash.Add(ChargeTransportSdlDm);
        hash.Add(MarketingMargin);
        hash.Add(CostLossesPr);
        hash.Add(RestrictionsRm);
        hash.Add(Cot);
        hash.Add(CfmjGfact);
        return hash.ToHashCode();
    }
}
