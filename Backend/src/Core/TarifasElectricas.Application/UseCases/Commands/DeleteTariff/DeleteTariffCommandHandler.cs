using System;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Commands.DeleteTariff;

/// <summary>
/// Handler para DeleteTariffCommand.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class DeleteTariffCommandHandler(
    IElectricityTariffRepository tariffs,
    IUnitOfWork unitOfWork)
{
    private readonly IElectricityTariffRepository _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<DeleteTariffResponse> Handle(DeleteTariffCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            // Obtener la tarifa existente
            var tariff = await _tariffs.GetByIdAsync(command.Id);

            if (tariff == null)
                throw new ApplicationCaseException($"Tarifa con ID {command.Id} no encontrada");

            // Eliminar
            await _tariffs.DeleteAsync(tariff);
            await _unitOfWork.SaveChangesAsync();

            // Retornar response
            return new DeleteTariffResponse(
                command.Id,
                true,
                "Tarifa eliminada exitosamente"
            );
        }, "Error al eliminar la tarifa");
    }
}
