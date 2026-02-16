using System;
using Xunit;
using TarifasElectricas.Domain.ValueObjects;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Test.Domain.ValueObjects;

/// <summary>
/// Tests para TariffCosts Value Object
/// Valida: creación, validaciones de costos, cálculos
/// </summary>
public class TariffCostsTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesTariffCosts()
    {
        // Arrange & Act
        var costs = new TariffCosts(
            810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);

        // Assert
        Assert.NotNull(costs);
        Assert.Equal(810.46m, costs.TotalCu);
        Assert.Equal(372.46m, costs.PurchaseCostG);
        Assert.Equal(56.03m, costs.ChargeTransportStnTm);
        Assert.Equal(280.91m, costs.ChargeTransportSdlDm);
        Assert.Equal(24.86m, costs.MarketingMargin);
        Assert.Equal(72.49m, costs.CostLossesPr);
        Assert.Equal(3.71m, costs.RestrictionsRm);
        Assert.Equal(0m, costs.Cot);
        Assert.Equal(9.813m, costs.CfmjGfact);
    }

    [Fact]
    public void Constructor_WithAllNullValues_CreatesTariffCosts()
    {
        // Arrange & Act
        var costs = new TariffCosts(null, null, null, null, null, null, null, null, null);

        // Assert
        Assert.NotNull(costs);
        Assert.Null(costs.TotalCu);
        Assert.Null(costs.PurchaseCostG);
    }

    [Fact]
    public void Constructor_WithNegativeCost_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffCosts(
                -810.46m,  // Negativo
                372.46m,
                56.03m,
                280.91m,
                24.86m,
                72.49m,
                3.71m,
                0m,
                9.813m));
    }

    [Fact]
    public void Constructor_WithNegativeMarketingMargin_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffCosts(
                810.46m,
                372.46m,
                56.03m,
                280.91m,
                -24.86m,  // Negativo
                72.49m,
                3.71m,
                0m,
                9.813m));
    }

    [Fact]
    public void Constructor_WithNegativeTransportCharge_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffCosts(
                810.46m,
                372.46m,
                -56.03m,  // Negativo
                280.91m,
                24.86m,
                72.49m,
                3.71m,
                0m,
                9.813m));
    }

    [Fact]
    public void CalculateTotal_SumsAllComponents()
    {
        // Arrange
        var costs = new TariffCosts(
            100m,   // TotalCu
            40m,    // PurchaseCostG
            30m,    // ChargeTransportStnTm
            20m,    // ChargeTransportSdlDm
            10m,    // MarketingMargin
            5m,     // CostLossesPr
            3m,     // RestrictionsRm
            2m,     // Cot
            1m);    // CfmjGfact

        // Act
        var total = costs.CalculateTotal();

        // Assert
        Assert.Equal(211m, total);
    }

    [Fact]
    public void CalculateTotal_WithNullComponents_TreatsAsZero()
    {
        // Arrange
        var costs = new TariffCosts(
            100m,   // TotalCu
            null,   // PurchaseCostG (null)
            30m,    // ChargeTransportStnTm
            null,   // ChargeTransportSdlDm (null)
            10m,    // MarketingMargin
            null,   // CostLossesPr (null)
            3m,     // RestrictionsRm
            null,   // Cot (null)
            1m);    // CfmjGfact

        // Act
        var total = costs.CalculateTotal();

        // Assert
        Assert.Equal(144m, total); // 100 + 30 + 10 + 3 + 1
    }

    [Fact]
    public void CalculateTotal_WithAllNullValues_ReturnsZero()
    {
        // Arrange
        var costs = new TariffCosts(null, null, null, null, null, null, null, null, null);

        // Act
        var total = costs.CalculateTotal();

        // Assert
        Assert.Equal(0m, total);
    }

    [Fact]
    public void Equality_WithSameValues_AreEqual()
    {
        // Arrange
        var costs1 = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);
        var costs2 = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);

        // Act & Assert
        Assert.Equal(costs1, costs2);
    }

    [Fact]
    public void Equality_WithDifferentValues_AreNotEqual()
    {
        // Arrange
        var costs1 = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);
        var costs2 = new TariffCosts(825.50m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);

        // Act & Assert
        Assert.NotEqual(costs1, costs2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ProduceSameHash()
    {
        // Arrange
        var costs1 = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);
        var costs2 = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);

        // Act
        var hash1 = costs1.GetHashCode();
        var hash2 = costs2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }
}