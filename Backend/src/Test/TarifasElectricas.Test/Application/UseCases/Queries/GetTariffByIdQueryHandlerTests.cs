using System;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Queries.GetTariffById;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

public class GetTariffByIdQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetTariffByIdQueryHandler _handler;

    public GetTariffByIdQueryHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new GetTariffByIdQueryHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidId_ReturnsTariff()
    {
        // Arrange
        var period = new TariffPeriod(2025, 2, "BT1", "Baja Tensión", "Distribuidora A");
        var costs = new TariffCosts(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);
        var tariff = new ElectricityTariff(period, costs);

        _unitOfWork.ElectricityTariffs.GetByIdAsync(tariff.Id)
            .Returns(Task.FromResult((ElectricityTariff?)tariff));

        var query = new GetTariffByIdQuery(tariff.Id);

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(tariff.Id, response.Id);
        Assert.Equal(2025, response.Year);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ThrowsApplicationCaseException()
    {
        // Arrange
        _unitOfWork.ElectricityTariffs.GetByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult((ElectricityTariff?)null));

        var query = new GetTariffByIdQuery(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
    }
}

