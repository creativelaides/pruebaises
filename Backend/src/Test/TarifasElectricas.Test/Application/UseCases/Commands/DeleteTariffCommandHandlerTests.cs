using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.UseCases.Commands.DeleteTariff;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Commands;

public class DeleteTariffCommandHandlerTests
{
    private readonly IElectricityTariffRepository _tariffs;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteTariffCommandHandler _handler;

    public DeleteTariffCommandHandlerTests()
    {
        _tariffs = Substitute.For<IElectricityTariffRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteTariffCommandHandler(_tariffs, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidId_DeletesTariffAndReturnsSuccess()
    {
        // Arrange
        var tariffId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var existingTariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
            companyId);

        var command = new DeleteTariffCommand(tariffId);

        // Mock: Obtener tarifa existente
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(existingTariff));

        // Act
        var response = await _handler.Handle(command);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(tariffId, response.Id);
        Assert.True(response.Success);
        Assert.Contains("eliminada", response.Message.ToLower());

        // Verify
        await _tariffs.Received(1).DeleteAsync(Arg.Any<ElectricityTariff>());
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WithNonExistentTariff_ThrowsApplicationCaseException()
    {
        // Arrange
        var tariffId = Guid.NewGuid();
        var command = new DeleteTariffCommand(tariffId);

        // Mock: Tarifa NO existe
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(command));
        Assert.Contains("Tarifa con ID", ex.Message);
    }
}
