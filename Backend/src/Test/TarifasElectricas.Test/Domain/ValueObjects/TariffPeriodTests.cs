using System;
using TarifasElectricas.Domain.Exceptions;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Test.Domain.ValueObjects;

public class TariffPeriodTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesTariffPeriod()
    {
        // Arrange & Act
        var tariffPeriod = new TariffPeriod(2025, 2, "BT1-Residencial", "Baja Tensión", "Distribuidora A");

        // Assert
        Assert.Equal(2025, tariffPeriod.Year);
        Assert.Equal(2, tariffPeriod.Month);
    }

    [Fact]
    public void Constructor_WithInvalidYear_ThrowsDomainRuleException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleException>(
            () => new TariffPeriod(1800, 2, "BT1", "Baja Tensión", "Distribuidora A")
        );
        Assert.Contains("Año inválido", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidMonth_ThrowsDomainRuleException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleException>(
            () => new TariffPeriod(2025, 13, "BT1", "Baja Tensión", "Distribuidora A")
        );
        Assert.Contains("Mes inválido", exception.Message);
    }
}
