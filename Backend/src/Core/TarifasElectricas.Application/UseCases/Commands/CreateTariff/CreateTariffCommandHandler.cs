namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

using System;
using System.Threading.Tasks;
using Mapster;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
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
    private readonly IElectricityTariffRepository _tariffs;
    private readonly ICompanyRepository _companies;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTariffCommandHandler(
        IElectricityTariffRepository tariffs,
        ICompanyRepository companies,
        IUnitOfWork unitOfWork)
    {
        _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));
        _companies = companies ?? throw new ArgumentNullException(nameof(companies));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateTariffResponse> Handle(CreateTariffCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            // ✅ NUEVO: Validar que Company existe
            var company = await _companies.GetByIdAsync(command.CompanyId);
            if (company == null)
                throw new ApplicationCaseException(
                    $"Empresa con ID {command.CompanyId} no encontrada");

            // Verificar duplicado por período
            var existing = await _tariffs
                .GetByPeriodAsync(command.Year, command.Period!);

            if (existing != null)
                throw new ApplicationCaseException(
                    $"Ya existe una tarifa para el período {command.Year}-{command.Period}");

            // ✅ ACTUALIZAR: Crear TariffPeriod SIN Month, con TariffOperator
            var period = new TariffPeriod(
                command.Year,
                command.Period!,
                command.Level!,
                command.TariffOperator!,
                DateTime.UtcNow.Year);

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
            await _tariffs.AddAsync(tariff);
            await _unitOfWork.SaveChangesAsync();

            // Retornar response
            return tariff.Adapt<CreateTariffResponse>();
        }, "Error al crear la tarifa");
    }
}
