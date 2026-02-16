using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.UseCases.Commands.UpdateTariff;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Commands;

public class UpdateTariffCommandHandlerTests
{
    private readonly IElectricityTariffRepository _tariffs;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateTariffCommandHandler _handler;

    public UpdateTariffCommandHandlerTests()
    {
        _tariffs = Substitute.For<IElectricityTariffRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateTariffCommandHandler(_tariffs, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_UpdatesTariffAndReturnsResponse()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var existingTariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
            companyId);
        var tariffId = existingTariff.Id;

        var command = new UpdateTariffCommand(
            Id: tariffId,
            TotalCu: 825.50m,  // Valor actualizado
            PurchaseCostG: 380.00m,
            ChargeTransportStnTm: 56.03m,
            ChargeTransportSdlDm: 290.00m,
            MarketingMargin: 25.00m,
            CostLossesPr: 75.00m,
            RestrictionsRm: 3.71m,
            Cot: 0m,
            CfmjGfact: 9.813m);

        // Mock: Obtener tarifa existente
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(existingTariff));

        // Act
        var response = await _handler.Handle(command);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(tariffId, response.Id);
        Assert.Equal(2025, response.Year);
        Assert.Equal("Enero", response.Period);
        var expectedTotal = 825.50m + 380.00m + 56.03m + 290.00m + 25.00m + 75.00m + 3.71m + 0m + 9.813m;
        Assert.Equal(expectedTotal, response.TotalCosts);  // Verificar que se actualizó
        Assert.Equal(companyId, response.CompanyId);

        // Verify
        await _tariffs.Received(1).UpdateAsync(Arg.Any<ElectricityTariff>());
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WithNonExistentTariff_ThrowsApplicationCaseException()
    {
        // Arrange
        var tariffId = Guid.NewGuid();
        var command = new UpdateTariffCommand(
            Id: tariffId,
            TotalCu: 825.50m,
            PurchaseCostG: 380.00m,
            ChargeTransportStnTm: 56.03m,
            ChargeTransportSdlDm: 290.00m,
            MarketingMargin: 25.00m,
            CostLossesPr: 75.00m,
            RestrictionsRm: 3.71m,
            Cot: 0m,
            CfmjGfact: 9.813m);

        // Mock: Tarifa NO existe
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(command));
        Assert.Contains("Tarifa con ID", ex.Message);
    }
}
