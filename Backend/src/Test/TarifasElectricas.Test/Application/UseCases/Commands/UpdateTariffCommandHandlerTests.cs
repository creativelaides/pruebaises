using System;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Commands.UpdateTariff;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Commands;

public class UpdateTariffCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateTariffCommandHandler _handler;

    public UpdateTariffCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateTariffCommandHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_UpdatesTariffAndReturnsResponse()
    {
        // Arrange
        var period = new TariffPeriod(2025, 2, "BT1", "Baja Tensión", "Distribuidora A");
        var costs = new TariffCosts(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);
        var existingTariff = new ElectricityTariff(period, costs);

        _unitOfWork.ElectricityTariffs.GetByIdAsync(existingTariff.Id)
            .Returns(Task.FromResult((ElectricityTariff?)existingTariff));

        _unitOfWork.ElectricityTariffs.UpdateAsync(Arg.Any<ElectricityTariff>())
            .Returns(Task.CompletedTask);

        _unitOfWork.SaveChangesAsync().Returns(Task.FromResult(1));

        var command = new UpdateTariffCommand(
            existingTariff.Id,
            0.50m, 0.13m, 0.09m, 0.06m, 0.03m, 0.02m, 0.01m, 0.11m, 0.05m
        );

        // Act
        var response = await _handler.Handle(command);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(existingTariff.Id, response.Id);
        Assert.Equal(1.00m, response.TotalCosts);
    }

    [Fact]
    public async Task Handle_WithNonExistentTariff_ThrowsApplicationCaseException()
    {
        // Arrange
        _unitOfWork.ElectricityTariffs.GetByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult((ElectricityTariff?)null));

        var command = new UpdateTariffCommand(
            Guid.NewGuid(),
            0.50m, 0.13m, 0.09m, 0.06m, 0.03m, 0.02m, 0.01m, 0.11m, 0.05m
        );

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(command));
    }
}

