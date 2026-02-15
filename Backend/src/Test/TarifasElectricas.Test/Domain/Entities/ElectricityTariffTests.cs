using System;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Domain.Entities;

public class ElectricityTariffTests
{
    private readonly TariffPeriod _validPeriod =
            new(2025, 2, "BT1-Residencial", "Baja Tensión", "Distribuidora A");

    private readonly TariffCosts _validCosts =
        new(0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m);

    [Fact]
    public void Constructor_WithValidData_CreatesElectricityTariff()
    {
        // Act
        var tariff = new ElectricityTariff(_validPeriod, _validCosts);

        // Assert
        Assert.NotEqual(Guid.Empty, tariff.Id);
        Assert.Equal(_validPeriod, tariff.Period);
        Assert.Equal(_validCosts, tariff.Costs);
    }

    [Fact]
    public void UpdateCosts_UpdatesValuesCorrectly()
    {
        // Arrange
        var tariff = new ElectricityTariff(_validPeriod, _validCosts);
        var newCosts = new TariffCosts(0.50m, 0.13m, 0.09m, 0.06m, 0.03m, 0.02m, 0.01m, 0.11m, 0.05m);

        // Act
        tariff.UpdateCosts(newCosts);

        // Assert
        Assert.Equal(newCosts, tariff.Costs);
    }

    [Fact]
    public void GetTotalCosts_ReturnsSumOfAllComponents()
    {
        // Arrange
        var tariff = new ElectricityTariff(_validPeriod, _validCosts);

        // Act
        var result = tariff.GetTotalCosts();

        // Assert
        Assert.Equal(0.90m, result);
    }
}
