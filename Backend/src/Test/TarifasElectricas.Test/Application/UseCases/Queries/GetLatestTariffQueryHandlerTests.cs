using System;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

public class GetLatestTariffQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetLatestTariffQueryHandler _handler;

    public GetLatestTariffQueryHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new GetLatestTariffQueryHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WithLatestTariff_ReturnsTariff()
    {
        // Arrange
        var period = new TariffPeriod(2025, 2, "BT1", "Baja Tensión", "Distribuidora A");
        var costs = new TariffCosts(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);
        var latestTariff = new ElectricityTariff(period, costs);

        _unitOfWork.ElectricityTariffs.GetLatestAsync()
            .Returns(Task.FromResult((ElectricityTariff?)latestTariff));

        var query = new GetLatestTariffQuery();

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(latestTariff.Id, response.Id);
        Assert.Equal(2025, response.Year);
    }

    [Fact]
    public async Task Handle_WithNoTariffs_ThrowsApplicationCaseException()
    {
        // Arrange
        _unitOfWork.ElectricityTariffs.GetLatestAsync()
            .Returns(Task.FromResult((ElectricityTariff?)null));

        var query = new GetLatestTariffQuery();

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
    }
}

