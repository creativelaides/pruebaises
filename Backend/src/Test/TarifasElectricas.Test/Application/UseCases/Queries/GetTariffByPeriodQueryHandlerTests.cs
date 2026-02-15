using System;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

public class GetTariffByPeriodQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetTariffByPeriodQueryHandler _handler;

    public GetTariffByPeriodQueryHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new GetTariffByPeriodQueryHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidPeriod_ReturnsTariff()
    {
        // Arrange
        var period = new TariffPeriod(2025, 2, "BT1", "Baja Tensión", "Distribuidora A");
        var costs = new TariffCosts(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);
        var tariff = new ElectricityTariff(period, costs);

        _unitOfWork.ElectricityTariffs.GetByPeriodAsync(2025, 2)
            .Returns(Task.FromResult((ElectricityTariff?)tariff));

        var query = new GetTariffByPeriodQuery(2025, 2);

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2025, response.Year);
        Assert.Equal(2, response.Month);
    }

    [Fact]
    public async Task Handle_WithNonExistentPeriod_ThrowsApplicationCaseException()
    {
        // Arrange
        _unitOfWork.ElectricityTariffs.GetByPeriodAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(Task.FromResult((ElectricityTariff?)null));

        var query = new GetTariffByPeriodQuery(2025, 3);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
    }
}

