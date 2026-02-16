using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.UseCases.Commands.CreateTariff;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Commands;

public class CreateTariffCommandHandlerTests
{
    private readonly IElectricityTariffRepository _tariffs;
    private readonly ICompanyRepository _companies;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateTariffCommandHandler _handler;

    public CreateTariffCommandHandlerTests()
    {
        _tariffs = Substitute.For<IElectricityTariffRepository>();
        _companies = Substitute.For<ICompanyRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateTariffCommandHandler(_tariffs, _companies, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesAndReturnsTariff()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = new Company("ENEL Bogotá - Cundinamarca");

        var command = new CreateTariffCommand(
            Year: 2025,
            Period: "Enero",
            Level: "Nivel 1 (Propiedad OR)",
            TariffOperator: "ENEL Bogotá - Cundinamarca",
            CompanyId: companyId,
            TotalCu: 810.46m,
            PurchaseCostG: 372.46m,
            ChargeTransportStnTm: 56.03m,
            ChargeTransportSdlDm: 280.91m,
            MarketingMargin: 24.86m,
            CostLossesPr: 72.49m,
            RestrictionsRm: 3.71m,
            Cot: 0m,
            CfmjGfact: 9.813m);

        // Mock: Company existe
        _companies.GetByIdAsync(companyId).Returns(Task.FromResult<Company?>(company));

        // Mock: No existe tarifa duplicada
        _tariffs.GetByPeriodAsync(2025, "Enero").Returns(Task.FromResult<ElectricityTariff?>(null));

        // Act
        var response = await _handler.Handle(command);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2025, response.Year);
        Assert.Equal("Enero", response.Period);
        Assert.Equal("Nivel 1 (Propiedad OR)", response.Level);
        Assert.Equal("ENEL Bogotá - Cundinamarca", response.TariffOperator);
        Assert.Equal(companyId, response.CompanyId);
        var expectedTotal = 810.46m + 372.46m + 56.03m + 280.91m + 24.86m + 72.49m + 3.71m + 0m + 9.813m;
        Assert.Equal(expectedTotal, response.TotalCosts);
        Assert.NotEqual(Guid.Empty, response.Id);

        // Verify
        await _tariffs.Received(1).AddAsync(Arg.Any<ElectricityTariff>());
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WithNonExistentCompany_ThrowsApplicationCaseException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var command = new CreateTariffCommand(
            Year: 2025,
            Period: "Enero",
            Level: "Nivel 1 (Propiedad OR)",
            TariffOperator: "ENEL Bogotá - Cundinamarca",
            CompanyId: companyId,
            TotalCu: 810.46m,
            PurchaseCostG: 372.46m,
            ChargeTransportStnTm: 56.03m,
            ChargeTransportSdlDm: 280.91m,
            MarketingMargin: 24.86m,
            CostLossesPr: 72.49m,
            RestrictionsRm: 3.71m,
            Cot: 0m,
            CfmjGfact: 9.813m);

        // Mock: Company NO existe
        _companies.GetByIdAsync(companyId).Returns(Task.FromResult<Company?>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(command));
        Assert.Contains("Empresa con ID", ex.Message);
    }

    [Fact]
    public async Task Handle_WithDuplicatePeriod_ThrowsApplicationCaseException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = new Company("ENEL Bogotá - Cundinamarca");
        var existingTariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
            companyId);

        var command = new CreateTariffCommand(
            Year: 2025,
            Period: "Enero",
            Level: "Nivel 1 (Propiedad OR)",
            TariffOperator: "ENEL Bogotá - Cundinamarca",
            CompanyId: companyId,
            TotalCu: 810.46m,
            PurchaseCostG: 372.46m,
            ChargeTransportStnTm: 56.03m,
            ChargeTransportSdlDm: 280.91m,
            MarketingMargin: 24.86m,
            CostLossesPr: 72.49m,
            RestrictionsRm: 3.71m,
            Cot: 0m,
            CfmjGfact: 9.813m);

        // Mock: Company existe
        _companies.GetByIdAsync(companyId).Returns(Task.FromResult<Company?>(company));

        // Mock: Tarifa duplicada existe
        _tariffs.GetByPeriodAsync(2025, "Enero").Returns(Task.FromResult<ElectricityTariff?>(existingTariff));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(command));
        Assert.Contains("Ya existe una tarifa", ex.Message);
    }
}
