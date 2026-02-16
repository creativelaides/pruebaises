using System;
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
    public async Task Handle_WithValidPeriod_ReturnsTariff()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var tariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
            companyId);

        var query = new GetTariffByPeriodQuery(2025, "Enero");

        // Mock: Pasar Year (int) y Period (string)
        _tariffs.GetByPeriodAsync(2025, "Enero").Returns(Task.FromResult<ElectricityTariff?>(tariff));

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
    public async Task Handle_WithNonExistentPeriod_ThrowsApplicationCaseException()
    {
        // Arrange
        var query = new GetTariffByPeriodQuery(2025, "Febrero");

        // Mock: Tarifa no existe
        _tariffs.GetByPeriodAsync(2025, "Febrero").Returns(Task.FromResult<ElectricityTariff?>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
        Assert.Contains("Tarifa no encontrada", ex.Message);
    }
}
