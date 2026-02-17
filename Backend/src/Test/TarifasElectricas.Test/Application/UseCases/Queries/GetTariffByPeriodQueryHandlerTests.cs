using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

public class GetTariffByPeriodQueryHandlerTests
{
    private readonly IElectricityTariffRepository _tariffs;
    private readonly GetTariffByPeriodQueryHandler _handler;

    public GetTariffByPeriodQueryHandlerTests()
    {
        _tariffs = Substitute.For<IElectricityTariffRepository>();
        _handler = new GetTariffByPeriodQueryHandler(_tariffs);
    }

    [Fact]
    public async Task Handle_WithValidPeriod_ReturnsTariffs()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var tariff1 = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
            companyId);
        var tariff2 = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "NIVEL II", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(691.89m, 372.46m, 56.03m, 168.8m, 66.56m, 24.32m, 3.71m, 41.7m, 9.813m),
            companyId);

        var query = new GetTariffByPeriodQuery(2025, "Enero", null, null);

        _tariffs.GetByFiltersAsync(2025, "Enero", null, null)
            .Returns(Task.FromResult<IEnumerable<ElectricityTariff>>(new[] { tariff1, tariff2 }));

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Tariffs.Count());
        Assert.All(response.Tariffs, t =>
        {
            Assert.Equal(2025, t.Year);
            Assert.Equal("Enero", t.Period);
            Assert.Equal(companyId, t.CompanyId);
        });
    }

    [Fact]
    public async Task Handle_WithNonExistentPeriod_ThrowsApplicationCaseException()
    {
        // Arrange
        var query = new GetTariffByPeriodQuery(2025, "Febrero", null, null);

        _tariffs.GetByFiltersAsync(2025, "Febrero", null, null)
            .Returns(Task.FromResult<IEnumerable<ElectricityTariff>>(Array.Empty<ElectricityTariff>()));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
        Assert.Contains("No hay tarifas", ex.Message);
    }
}
