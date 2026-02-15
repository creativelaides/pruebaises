namespace TarifasElectricas.Application.UseCases.Commands.UpdateTariff;

/// <summary>
/// Respuesta del comando UpdateTariffCommand.
/// Record - Inmutable.
/// </summary>
public record UpdateTariffResponse(
    Guid Id,
    int Year,
    int Month,
    string? Period,
    string? Level,
    string? Operator,
    decimal TotalCosts,
    DateTime UpdatedAt);
