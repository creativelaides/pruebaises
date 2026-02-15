using System;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Commands.CreateTariff;

namespace TarifasElectricas.Application.UseCases.Commands.DeleteTariff;

/// <summary>
/// Handler para DeleteTariffCommand.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class DeleteTariffCommandHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<DeleteTariffResponse> Handle(DeleteTariffCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        try
        {
            // Obtener la tarifa existente
            var tariff = await _unitOfWork.ElectricityTariffs.GetByIdAsync(command.Id);

            if (tariff == null)
                throw new ApplicationCaseException($"Tarifa con ID {command.Id} no encontrada");

            // Eliminar
            await _unitOfWork.ElectricityTariffs.DeleteAsync(tariff);
            await _unitOfWork.SaveChangesAsync();

            // Retornar response
            return new DeleteTariffResponse(
                command.Id,
                true,
                "Tarifa eliminada exitosamente"
            );
        }
        catch (ApplicationCaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationCaseException($"Error al eliminar la tarifa: {ex.Message}", ex);
        }
    }
}