namespace TarifasElectricas.Application.UseCases.Queries.GetTariffById;

/// <summary>
/// Respuesta de la query GetTariffByIdQuery.
/// Record - Inmutable.
/// </summary>
public record GetTariffByIdResponse(
    Guid Id,
    int Year,
    int Month,
    string? Period,
    string? Level,
    string? Operator,
    decimal TotalCosts,
    DateTime CreatedAt);
