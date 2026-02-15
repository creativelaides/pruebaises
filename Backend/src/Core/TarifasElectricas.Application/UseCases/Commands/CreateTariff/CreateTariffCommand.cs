using System;

namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Comando para crear una nueva tarifa eléctrica
/// Record - Inmutable, contiene los datos de entrada
/// </summary>
public record CreateTariffCommand
(
    int Year,
    int Month,
    string? Period,
    string? Level,
    string? Operator,
    decimal? TotalCu,
    decimal? PurchaseCostG,
    decimal? ChargeTransportStnTm,
    decimal? ChargeTransportSdlDm,
    decimal? MarketingMargin,
    decimal? CostLossesPr,
    decimal? RestrictionsRm,
    decimal? Cot,
    decimal? CfmjGfact
);
