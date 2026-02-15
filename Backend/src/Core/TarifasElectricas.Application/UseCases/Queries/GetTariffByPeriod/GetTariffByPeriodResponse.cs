namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Respuesta de la query GetTariffByPeriodQuery.
/// Record - Inmutable.
/// </summary>
public record GetTariffByPeriodResponse(
    Guid Id,
    int Year,
    int Month,
    string? Period,
    string? Level,
    string? Operator,
    decimal TotalCosts,
    DateTime CreatedAt);
