namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Respuesta del comando DeleteTariffCommand.
/// Record - Inmutable.
/// </summary>
public record DeleteTariffResponse(
    Guid Id,
    bool Success,
    string Message);
