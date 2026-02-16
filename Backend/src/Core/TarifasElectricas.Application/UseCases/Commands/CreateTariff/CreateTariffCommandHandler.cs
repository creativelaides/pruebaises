namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

using System;
using System.Threading.Tasks;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Exceptions;
using TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Handler para CreateTariffCommand
/// Descubierto automáticamente por WolverineFx
/// 
/// Responsabilidades:
/// 1. Validar que la Company existe
/// 2. Verificar que no existe tarifa duplicada
/// 3. Crear TariffPeriod con datos del comando
/// 4. Crear TariffCosts con 9 componentes
/// 5. Crear ElectricityTariff con CompanyId
/// 6. Persistir en BD
/// </summary>
public class CreateTariffCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTariffCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateTariffResponse> Handle(CreateTariffCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            // ✅ NUEVO: Validar que Company existe
            var company = await _unitOfWork.Companies.GetByIdAsync(command.CompanyId);
            if (company == null)
                throw new ApplicationCaseException(
                    $"Empresa con ID {command.CompanyId} no encontrada");

            // Verificar duplicado por período
            var existing = await _unitOfWork.ElectricityTariffs
                .GetByPeriodAsync(command.Year, command.Period ?? "Unknown");

            if (existing != null)
                throw new ApplicationCaseException(
                    $"Ya existe una tarifa para el período {command.Year}-{command.Period}");

            // ✅ ACTUALIZAR: Crear TariffPeriod SIN Month, con TariffOperator
            var period = new TariffPeriod(
                command.Year,
                command.Period ?? "Unknown",
                command.Level ?? "Unknown",
                command.TariffOperator ?? "Unknown");

            // Crear TariffCosts
            var costs = new TariffCosts(
                command.TotalCu,
                command.PurchaseCostG,
                command.ChargeTransportStnTm,
                command.ChargeTransportSdlDm,
                command.MarketingMargin,
                command.CostLossesPr,
                command.RestrictionsRm,
                command.Cot,
                command.CfmjGfact);

            // ✅ ACTUALIZAR: Crear ElectricityTariff CON CompanyId
            var tariff = new ElectricityTariff(period, costs, command.CompanyId);

            // Persistir
            await _unitOfWork.ElectricityTariffs.AddAsync(tariff);
            await _unitOfWork.SaveChangesAsync();

            // Retornar response
            return new CreateTariffResponse(
                tariff.Id,
                tariff.Period.Year,
                tariff.Period.Period,
                tariff.Period.Level,
                tariff.Period.TariffOperator,
                command.CompanyId,
                tariff.GetTotalCosts(),
                tariff.CreatedAt);
        }
        catch (DomainRuleException ex)
        {
            throw new ApplicationCaseException(
                $"Error de validación en el dominio: {ex.Message}", ex);
        }
        catch (ApplicationCaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationCaseException(
                $"Error al crear la tarifa: {ex.Message}", ex);
        }
    }
}