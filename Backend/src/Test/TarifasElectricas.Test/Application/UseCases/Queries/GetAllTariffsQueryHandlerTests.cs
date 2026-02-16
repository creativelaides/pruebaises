using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

public class GetAllTariffsQueryHandlerTests
{
    private readonly IElectricityTariffRepository _tariffs;
    private readonly GetAllTariffsQueryHandler _handler;

    public GetAllTariffsQueryHandlerTests()
    {
        _tariffs = Substitute.For<IElectricityTariffRepository>();
        _handler = new GetAllTariffsQueryHandler(_tariffs);
    }

    [Fact]
    public async Task Handle_WithTariffs_ReturnsAllTariffs()
    {
        // Arrange
        var companyId1 = Guid.NewGuid();
        var companyId2 = Guid.NewGuid();

        var tariffs = new List<ElectricityTariff>
        {
            new ElectricityTariff(
                new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
                new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
                companyId1),
            new ElectricityTariff(
                new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "CELSIA Colombia - Valle del Cauca", 2025),
                new TariffCosts(841m, 374.18m, 56.03m, 257.74m, 78.71m, 70.63m, 3.71m, 56.79m, 16.16m),
                companyId2)
        };

        var query = new GetAllTariffsQuery();

        // Mock
        _tariffs.GetAllAsync().Returns(Task.FromResult<IEnumerable<ElectricityTariff>>(tariffs));

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Tariffs);
        Assert.Equal(2, response.Tariffs.Count());

        var tariffList = response.Tariffs.ToList();
        Assert.All(tariffList, t => Assert.Equal(2025, t.Year));
        Assert.Contains(tariffList, t => t.CompanyId == companyId1);
        Assert.Contains(tariffList, t => t.CompanyId == companyId2);
    }

    [Fact]
    public async Task Handle_WithNoTariffs_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllTariffsQuery();
        var emptyList = new List<ElectricityTariff>();

        // Mock
        _tariffs.GetAllAsync().Returns(Task.FromResult<IEnumerable<ElectricityTariff>>(emptyList));

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Tariffs);
        Assert.Empty(response.Tariffs);
    }
}
