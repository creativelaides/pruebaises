namespace TarifasElectricas.Application.UseCases.Commands.DeleteTariff;

/// <summary>
/// Comando para eliminar una tarifa eléctrica.
/// Record - Inmutable.
/// </summary>
public record DeleteTariffCommand(Guid Id);
