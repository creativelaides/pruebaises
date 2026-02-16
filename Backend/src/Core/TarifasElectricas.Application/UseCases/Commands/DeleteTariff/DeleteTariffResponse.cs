namespace TarifasElectricas.Application.UseCases.Commands.DeleteTariff;

/// <summary>
/// Respuesta del comando DeleteTariffCommand.
/// Record - Inmutable.
/// </summary>
public record DeleteTariffResponse(
    Guid Id,
    bool Success,
    string Message);
