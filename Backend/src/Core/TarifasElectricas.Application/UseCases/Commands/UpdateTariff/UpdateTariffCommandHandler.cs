using System;
using Mapster;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Application.UseCases.Commands.UpdateTariff;

/// <summary>
/// Handler para UpdateTariffCommand.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class UpdateTariffCommandHandler(
    IElectricityTariffRepository tariffs,
    IUnitOfWork unitOfWork)
{
    private readonly IElectricityTariffRepository _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<UpdateTariffResponse> Handle(UpdateTariffCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            // Obtener la tarifa existente
            var tariff = await _tariffs.GetByIdAsync(command.Id);

            if (tariff == null)
                throw new ApplicationCaseException($"Tarifa con ID {command.Id} no encontrada");

            // Crear nuevos costos
            var newCosts = new TariffCosts(
                command.TotalCu,
                command.PurchaseCostG,
                command.ChargeTransportStnTm,
                command.ChargeTransportSdlDm,
                command.MarketingMargin,
                command.CostLossesPr,
                command.RestrictionsRm,
                command.Cot,
                command.CfmjGfact
            );

            // Actualizar costos
            tariff.UpdateCosts(newCosts);

            // Persistir
            await _tariffs.UpdateAsync(tariff);
            await _unitOfWork.SaveChangesAsync();

            // Retornar response
            return tariff.Adapt<UpdateTariffResponse>();
        }, "Error al actualizar la tarifa");
    }
}

