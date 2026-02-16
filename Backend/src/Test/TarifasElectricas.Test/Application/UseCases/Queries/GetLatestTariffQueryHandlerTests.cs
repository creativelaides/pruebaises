using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

public class GetLatestTariffQueryHandlerTests
{
    private readonly IElectricityTariffRepository _tariffs;
    private readonly GetLatestTariffQueryHandler _handler;

    public GetLatestTariffQueryHandlerTests()
    {
        _tariffs = Substitute.For<IElectricityTariffRepository>();
        _handler = new GetLatestTariffQueryHandler(_tariffs);
    }

    [Fact]
    public async Task Handle_WithLatestTariff_ReturnsTariff()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var tariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
            companyId);

        var query = new GetLatestTariffQuery();

        // Mock
        _tariffs.GetLatestAsync().Returns(Task.FromResult<ElectricityTariff?>(tariff));

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2025, response.Year);
        Assert.Equal("Enero", response.Period);
        Assert.Equal("Nivel 1 (Propiedad OR)", response.Level);
        Assert.Equal("ENEL Bogotá - Cundinamarca", response.TariffOperator);
        Assert.Equal(companyId, response.CompanyId);
        var expectedTotal = 810.46m + 372.46m + 56.03m + 280.91m + 24.86m + 72.49m + 3.71m + 0m + 9.813m;
        Assert.Equal(expectedTotal, response.TotalCosts);
    }

    [Fact]
    public async Task Handle_WithNoTariffs_ThrowsApplicationCaseException()
    {
        // Arrange
        var query = new GetLatestTariffQuery();

        // Mock: No hay tarifas
        _tariffs.GetLatestAsync().Returns(Task.FromResult<ElectricityTariff?>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
        Assert.Contains("No hay tarifas disponibles", ex.Message);
    }
}
