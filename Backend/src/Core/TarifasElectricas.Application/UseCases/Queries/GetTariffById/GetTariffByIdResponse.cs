namespace TarifasElectricas.Application.UseCases.Queries.GetTariffById;

/// <summary>
/// Respuesta de la query GetTariffByIdQuery.
/// Record - Inmutable.
/// </summary>
public record GetTariffByIdResponse(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime CreatedAt);
