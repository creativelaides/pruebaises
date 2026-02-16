namespace TarifasElectricas.Application.UseCases.Commands.UpdateTariff;

/// <summary>
/// Respuesta del comando UpdateTariffCommand.
/// Record - Inmutable.
/// </summary>
public record UpdateTariffResponse(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime UpdatedAt);
