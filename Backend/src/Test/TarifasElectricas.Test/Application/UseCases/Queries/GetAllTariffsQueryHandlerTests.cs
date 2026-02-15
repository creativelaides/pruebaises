using System;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

public class GetAllTariffsQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetAllTariffsQueryHandler _handler;

    public GetAllTariffsQueryHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new GetAllTariffsQueryHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WithTariffs_ReturnsAllTariffs()
    {
        // Arrange
        var period1 = new TariffPeriod(2025, 1, "BT1", "Baja Tensión", "Distribuidora A");
        var costs1 = new TariffCosts(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);
        var tariff1 = new ElectricityTariff(period1, costs1);

        var period2 = new TariffPeriod(2025, 2, "BT1", "Baja Tensión", "Distribuidora A");
        var costs2 = new TariffCosts(0.50m, 0.13m, 0.09m, 0.06m, 0.03m, 0.02m, 0.01m, 0.11m, 0.05m);
        var tariff2 = new ElectricityTariff(period2, costs2);

        var tariffs = new List<ElectricityTariff> { tariff1, tariff2 };

        _unitOfWork.ElectricityTariffs.GetAllAsync()
            .Returns(Task.FromResult((IEnumerable<ElectricityTariff>)tariffs));

        var query = new GetAllTariffsQuery();

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Tariffs);
        Assert.Equal(2, response.Tariffs.Count());
    }

    [Fact]
    public async Task Handle_WithNoTariffs_ReturnsEmptyList()
    {
        // Arrange
        _unitOfWork.ElectricityTariffs.GetAllAsync()
            .Returns(Task.FromResult((IEnumerable<ElectricityTariff>)new List<ElectricityTariff>()));

        var query = new GetAllTariffsQuery();

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Tariffs);
    }
}
