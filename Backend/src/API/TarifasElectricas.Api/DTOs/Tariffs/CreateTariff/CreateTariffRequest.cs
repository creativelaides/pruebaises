namespace TarifasElectricas.Api.DTOs.Tariffs.CreateTariff;

public record CreateTariffRequest(
    int Year,
    string Period,
    string Level,
    string TariffOperator,
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
