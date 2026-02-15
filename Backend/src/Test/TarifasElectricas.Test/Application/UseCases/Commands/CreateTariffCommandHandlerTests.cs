using System;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Commands.CreateTariff;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Commands;

public class CreateTariffCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateTariffCommandHandler _handler;

    public CreateTariffCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateTariffCommandHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesAndReturnsTariff()
    {
        // Arrange
        _unitOfWork.ElectricityTariffs.GetByPeriodAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(Task.FromResult((ElectricityTariff?)null));

        _unitOfWork.ElectricityTariffs.AddAsync(Arg.Any<ElectricityTariff>())
            .Returns(Task.CompletedTask);

        _unitOfWork.SaveChangesAsync().Returns(Task.FromResult(1));

        var command = new CreateTariffCommand(
            2025, 2, "BT1-Residencial", "Baja Tensión", "Distribuidora A",
            0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m
        );

        // Act
        var response = await _handler.Handle(command);

        // Assert
        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(2025, response.Year);
        Assert.Equal(2, response.Month);
    }

    [Fact]
    public async Task Handle_WithDuplicatePeriod_ThrowsApplicationCaseException()
    {
        // Arrange
        var period = new TariffPeriod(2025, 2, "BT1", "Baja Tensión", "Distribuidora A");
        var costs = new TariffCosts(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);
        var existingTariff = new ElectricityTariff(period, costs);

        _unitOfWork.ElectricityTariffs.GetByPeriodAsync(2025, 2)
            .Returns(Task.FromResult((ElectricityTariff?)existingTariff));

        var command = new CreateTariffCommand(
            2025, 2, "BT1-Residencial", "Baja Tensión", "Distribuidora A",
            0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m
        );

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(command));
    }
}
