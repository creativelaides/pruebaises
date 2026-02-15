using System;
using TarifasElectricas.Domain.Exceptions;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Domain.ValueObjects;

public class TariffCostsTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesTariffCosts()
    {
        // Arrange & Act
        var tariffCosts = new TariffCosts(
            0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m
        );

        // Assert
        Assert.NotNull(tariffCosts);
        Assert.Equal(0.45m, tariffCosts.TotalCu);
        Assert.Equal(0.12m, tariffCosts.PurchaseCostG);
    }

    [Fact]
    public void Constructor_WithNegativeValue_ThrowsDomainRuleException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleException>(
            () => new TariffCosts(
                -0.50m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m
            )
        );
        Assert.Contains("no puede ser negativo", exception.Message);
    }

    [Fact]
    public void CalculateTotalComponents_WithValidData_ReturnsCorrectSum()
    {
        // Arrange
        var tariffCosts = new TariffCosts(
            0.45m, 0.12m, 0.08m, 0.05m, 0.03m, 0.02m, 0.01m, 0.10m, 0.04m
        );

        // Act
        var result = tariffCosts.CalculateTotalComponents();

        // Assert
        Assert.Equal(0.90m, result);
    }
}
