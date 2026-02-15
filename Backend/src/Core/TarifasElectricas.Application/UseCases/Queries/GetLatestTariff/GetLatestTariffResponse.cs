using System;

namespace TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;

/// <summary>
/// Respuesta de la query GetLatestTariffQuery.
/// Record - Inmutable.
/// </summary>
public record GetLatestTariffResponse(
    Guid Id,
    int Year,
    int Month,
    string? Period,
    string? Level,
    string? Operator,
    decimal TotalCosts,
    DateTime CreatedAt);