using System;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Exceptions;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Application.UseCases.Commands.UpdateTariff;

/// <summary>
/// Handler para UpdateTariffCommand.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class UpdateTariffCommandHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<UpdateTariffResponse> Handle(UpdateTariffCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        try
        {
            // Obtener la tarifa existente
            var tariff = await _unitOfWork.ElectricityTariffs.GetByIdAsync(command.Id);

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
            await _unitOfWork.ElectricityTariffs.UpdateAsync(tariff);
            await _unitOfWork.SaveChangesAsync();

            // Retornar response
            return new UpdateTariffResponse(
                tariff.Id,
                tariff.Period.Year,
                tariff.Period.Month,
                tariff.Period.Period,
                tariff.Period.Level,
                tariff.Period.TariffOperator,
                tariff.GetTotalCosts(),
                tariff.DateUpdated
            );
        }
        catch (DomainRuleException ex)
        {
            throw new ApplicationCaseException($"Error de validación en el dominio: {ex.Message}", ex);
        }
        catch (ApplicationCaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationCaseException($"Error al actualizar la tarifa: {ex.Message}", ex);
        }
    }
}

