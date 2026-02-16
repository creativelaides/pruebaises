using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Application.UseCases.Queries;

/// <summary>
/// Tests para SimulateInvoiceQueryHandler
/// 
/// Responsabilidad del caso de uso:
/// 1. Obtener tarifa por ID
/// 2. Obtener empresa/operador
/// 3. Calcular desglose de costos basado en consumo kWh
/// 4. Crear componentes de factura educativos
/// 5. Retornar respuesta con datos de empresa y componentes
/// </summary>
public class SimulateInvoiceQueryHandlerTests
{
    private readonly IElectricityTariffRepository _tariffs;
    private readonly ICompanyRepository _companies;
    private readonly SimulateInvoiceQueryHandler _handler;

    public SimulateInvoiceQueryHandlerTests()
    {
        _tariffs = Substitute.For<IElectricityTariffRepository>();
        _companies = Substitute.For<ICompanyRepository>();
        _handler = new SimulateInvoiceQueryHandler(_tariffs, _companies);
    }

    [Fact]
    public async Task Handle_WithValidTariffAndConsumption_ReturnsInvoiceWithAllComponents()
    {
        // Arrange
        var company = new Company("ENEL Bogotá - Cundinamarca");
        var companyId = company.Id;
        var consumption = 100m; // 100 kWh

        var tariff = new ElectricityTariff(
            new TariffPeriod(
                2025,
                "Enero",
                "Nivel 1 (Propiedad OR)",
                "ENEL Bogotá - Cundinamarca",
                2025),
            new TariffCosts(
                totalCu: 810.46m,
                purchaseCostG: 372.46m,
                chargeTransportStnTm: 56.03m,
                chargeTransportSdlDm: 280.91m,
                marketingMargin: 24.86m,
                costLossesPr: 72.49m,
                restrictionsRm: 3.71m,
                cot: 0m,
                cfmjGfact: 9.813m),
            companyId);
        var tariffId = tariff.Id;

        var query = new SimulateInvoiceQuery(tariffId, consumption);

        // Mock
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(tariff));
        _companies.GetByIdAsync(companyId).Returns(Task.FromResult<Company?>(company));

        // Act
        var response = await _handler.Handle(query);

        // Assert - Validar estructura básica
        Assert.NotNull(response);
        Assert.Equal(tariffId, response.TariffId);
        Assert.Equal(companyId, response.CompanyId);
        Assert.Equal("ENEL Bogotá - Cundinamarca", response.CompanyName);
        Assert.Equal(consumption, response.KwhConsumption);
        Assert.NotNull(response.Components);

        // Assert - Validar cálculos
        // Consumo: 100 kWh * 810.46 COP/kWh = 81,046 COP
        var expectedConsumptionCost = consumption * 810.46m;
        Assert.Equal(expectedConsumptionCost, response.ConsumptionCost);

        // Transporte: 100 * (56.03 + 280.91) = 33,694 COP
        var expectedTransportCost = consumption * (56.03m + 280.91m);
        Assert.Equal(expectedTransportCost, response.TransportCost);

        // Marketing: 100 * 24.86 = 2,486 COP
        var expectedMarketingCost = consumption * 24.86m;
        Assert.Equal(expectedMarketingCost, response.MarketingCost);

        // Total
        var expectedTotal = expectedConsumptionCost + expectedTransportCost + expectedMarketingCost;
        Assert.Equal(expectedTotal, response.TotalCost);

        // Assert - Validar componentes educativos
        Assert.NotEmpty(response.Components);

        var consumptionComponent = response.Components.FirstOrDefault(c => c.Name == "Consumo de Energía");
        Assert.NotNull(consumptionComponent);
        Assert.Equal(expectedConsumptionCost, consumptionComponent.Value);
        Assert.Contains("electricidad", consumptionComponent.Explanation, StringComparison.OrdinalIgnoreCase);

        var transportComponent = response.Components.FirstOrDefault(c => c.Name == "Transporte y Distribución");
        Assert.NotNull(transportComponent);
        Assert.Equal(expectedTransportCost, transportComponent.Value);
        Assert.Contains("llevar", transportComponent.Explanation, StringComparison.OrdinalIgnoreCase);

        var marketingComponent = response.Components.FirstOrDefault(c => c.Name == "Comercialización");
        Assert.NotNull(marketingComponent);
        Assert.Equal(expectedMarketingCost, marketingComponent.Value);
        Assert.Contains("servicio", marketingComponent.Explanation, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Handle_WithLowConsumption_OnlyIncludesComponentsWithValue()
    {
        // Arrange: Consumo muy bajo para que algunos componentes sean 0
        var company = new Company("ENEL Bogotá - Cundinamarca");
        var companyId = company.Id;
        var consumption = 0.01m; // 0.01 kWh (casi nada)

        var tariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(
                totalCu: 810.46m,
                purchaseCostG: 372.46m,
                chargeTransportStnTm: 56.03m,
                chargeTransportSdlDm: 280.91m,
                marketingMargin: 24.86m,
                costLossesPr: 72.49m,
                restrictionsRm: 3.71m,
                cot: 0m,
                cfmjGfact: 9.813m),
            companyId);
        var tariffId = tariff.Id;
        var query = new SimulateInvoiceQuery(tariffId, consumption);

        // Mock
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(tariff));
        _companies.GetByIdAsync(companyId).Returns(Task.FromResult<Company?>(company));

        // Act
        var response = await _handler.Handle(query);

        // Assert
        Assert.NotNull(response);
        Assert.NotEmpty(response.Components);

        // Todos los componentes deberían tener valor > 0
        foreach (var component in response.Components)
        {
            Assert.True(component.Value > 0, $"Componente {component.Name} tiene valor <= 0");
        }
    }

    [Fact]
    public async Task Handle_WithNonExistentTariff_ThrowsApplicationCaseException()
    {
        // Arrange
        var tariffId = Guid.NewGuid();
        var query = new SimulateInvoiceQuery(tariffId, 100m);

        // Mock: Tarifa no existe
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
        Assert.Contains("Tarifa con ID", ex.Message);
    }

    [Fact]
    public async Task Handle_WithNonExistentCompany_ThrowsApplicationCaseException()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var tariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m),
            companyId);
        var tariffId = tariff.Id;
        var query = new SimulateInvoiceQuery(tariffId, 100m);

        // Mock
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(tariff));
        _companies.GetByIdAsync(companyId).Returns(Task.FromResult<Company?>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationCaseException>(() => _handler.Handle(query));
        Assert.Contains("Empresa no encontrada", ex.Message);
    }

    [Fact]
    public async Task Handle_CalculatesCorrectlyForDifferentConsumptions()
    {
        // Arrange: Probar con múltiples consumos para validar proporcionalidad
        var company = new Company("ENEL Bogotá - Cundinamarca");
        var companyId = company.Id;

        var tariff = new ElectricityTariff(
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025),
            new TariffCosts(
                totalCu: 100m,  // Valores simplificados para cálculo fácil
                purchaseCostG: 40m,
                chargeTransportStnTm: 30m,
                chargeTransportSdlDm: 20m,
                marketingMargin: 10m,
                costLossesPr: 0m,
                restrictionsRm: 0m,
                cot: 0m,
                cfmjGfact: 0m),
            companyId);
        var tariffId = tariff.Id;

        // Mock
        _tariffs.GetByIdAsync(tariffId).Returns(Task.FromResult<ElectricityTariff?>(tariff));
        _companies.GetByIdAsync(companyId).Returns(Task.FromResult<Company?>(company));

        var testConsumptions = new[] { 50m, 100m, 200m, 500m };

        // Act & Assert para cada consumo
        foreach (var consumption in testConsumptions)
        {
            var query = new SimulateInvoiceQuery(tariffId, consumption);
            var response = await _handler.Handle(query);

            Assert.Equal(consumption, response.KwhConsumption);
            Assert.Equal(consumption * 100m, response.ConsumptionCost);
            Assert.Equal(consumption * (30m + 20m), response.TransportCost);
            Assert.Equal(consumption * 10m, response.MarketingCost);
            Assert.Equal(consumption * 160m, response.TotalCost);
        }
    }
}
