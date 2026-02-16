namespace TarifasElectricas.Infrastructure.Utils.Mapping;

/// <summary>
/// Representa una fila del dataset ya normalizada y tipada.
/// </summary>
public readonly record struct TariffRow(
    int Year,
    string Period,
    string Level,
    string TariffOperator,
    decimal? TotalCu,
    decimal? PurchaseCostG,
    decimal? ChargeTransportStnTm,
    decimal? ChargeTransportSdlDm,
    decimal? MarketingMargin,
    decimal? CostLossesPr,
    decimal? RestrictionsRm,
    decimal? Cot,
    decimal? CfmjGfact);
