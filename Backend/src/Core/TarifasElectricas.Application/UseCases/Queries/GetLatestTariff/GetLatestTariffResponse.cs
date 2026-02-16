using System;

namespace TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;

/// <summary>
/// Respuesta de la query GetLatestTariffQuery.
/// Record - Inmutable.
/// </summary>
public record GetLatestTariffResponse(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime CreatedAt);