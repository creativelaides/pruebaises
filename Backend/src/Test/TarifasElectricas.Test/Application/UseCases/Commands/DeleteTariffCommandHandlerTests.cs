using System;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Commands.DeleteTariff;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Commands;

public class DeleteTariffCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteTariffCommandHandler _handler;

    public DeleteTariffCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteTariffCommandHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_DeletesTariffAndReturnsSuccess()
    {
        // Arrange
        var period = new TariffPeriod(2025, 2, "BT1", "Baja Tensión", "Distribuidora A");
        var costs = new TariffCosts(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);
        var existingTariff = new ElectricityTariff(period, costs);

        _unitOfWork.ElectricityTariffs.GetByIdAsync(existingTariff.Id)
            .Returns(Task.FromResult((ElectricityTariff?)existingTariff));

        _unitOfWork.ElectricityTariffs.DeleteAsync(Arg.Any<ElectricityTariff>())
            .Returns(Task.CompletedTask);

        _unitOfWork.SaveChangesAsync().Returns(Task.FromResult(1));

        var command = new DeleteTariffCommand(existingTariff.Id);

        // Act
        var response = await _handler.Handle(command);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal(existingTariff.Id, response.Id);
    }

    [Fact]
    public async Task Handle_WithNonExistentTariff_ThrowsApplicationCaseException()
    {
        // Arrange
        _unitOfWork.ElectricityTariffs.GetByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult((ElectricityTariff?)null));

        var command = new DeleteTariffCommand(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(command));
    }
}

