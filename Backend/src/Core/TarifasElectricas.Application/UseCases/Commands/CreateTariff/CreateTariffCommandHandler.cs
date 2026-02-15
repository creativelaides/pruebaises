using System;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Exceptions;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Handler para CreateTariffCommand
/// WolverineFx lo descubre automáticamente por el método Handle/HandleAsync
/// </summary>
public class CreateTariffCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTariffCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// WolverineFx invoca este método automáticamente
    /// </summary>
    public async Task<CreateTariffResponse> Handle(CreateTariffCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        try
        {
            // Verificar si ya existe una tarifa para ese período
            var existingTariff = await _unitOfWork.ElectricityTariffs
                .GetByPeriodAsync(command.Year, command.Month);

            if (existingTariff != null)
                throw new ApplicationCaseException(
                    $"Ya existe una tarifa para el período {command.Year}-{command.Month:D2}");

            // Crear Value Objects (las validaciones se hacen aquí en el Domain)
            var period = new TariffPeriod(
                command.Year,
                command.Month,
                command.Period,
                command.Level,
                command.Operator
            );

            var costs = new TariffCosts(
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

            // Crear la entidad
            var tariff = new ElectricityTariff(period, costs);

            // Persistir
            await _unitOfWork.ElectricityTariffs.AddAsync(tariff);
            await _unitOfWork.SaveChangesAsync();

            // Retornar response
            return new CreateTariffResponse(
                tariff.Id,
                tariff.Period.Year,
                tariff.Period.Month,
                tariff.Period.Period,
                tariff.Period.Level,
                tariff.Period.TariffOperator,
                tariff.GetTotalCosts(),
                tariff.CreatedAt
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
            throw new ApplicationCaseException($"Error al crear la tarifa: {ex.Message}", ex);
        }
    }
}

