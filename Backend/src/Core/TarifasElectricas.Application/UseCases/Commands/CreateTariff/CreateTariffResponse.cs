using System;

namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Respuesta del comando CreateTariffCommand
/// Record - Inmutable, salida estructurada
/// </summary>
public record CreateTariffResponse
(
    Guid Id,
    int Year,
    int Month,
    string? Period,
    string? Level,
    string? Operator,
    decimal TotalCosts,
    DateTime CreatedAt
);
