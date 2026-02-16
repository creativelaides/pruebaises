using System;
using Xunit;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Test.Domain.Entities;

/// <summary>
/// Tests para ElectricityTariff Entity
/// Valida: creación, validaciones, métodos de cálculo
/// </summary>
public class ElectricityTariffTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesElectricityTariff()
    {
        // Arrange
        var period = new TariffPeriod(
            2025,
            "Enero",
            "Nivel 1 (Propiedad OR)",
            "ENEL Bogotá - Cundinamarca",
            2025);

        var costs = new TariffCosts(
            810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);

        var companyId = Guid.NewGuid();

        // Act
        var tariff = new ElectricityTariff(period, costs, companyId);

        // Assert
        Assert.NotNull(tariff);
        Assert.NotEqual(Guid.Empty, tariff.Id);
        Assert.Equal(period, tariff.Period);
        Assert.Equal(costs, tariff.Costs);
        Assert.Equal(companyId, tariff.CompanyId);
        Assert.NotEqual(default, tariff.CreatedAt);
        Assert.NotEqual(default, tariff.DateUpdated);
    }

    [Fact]
    public void Constructor_WithNullPeriod_ThrowsDomainRuleException()
    {
        // Arrange
        var costs = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);
        var companyId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new ElectricityTariff(null!, costs, companyId));
    }

    [Fact]
    public void Constructor_WithNullCosts_ThrowsDomainRuleException()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var companyId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new ElectricityTariff(period, null!, companyId));
    }

    [Fact]
    public void Constructor_WithEmptyCompanyId_ThrowsDomainRuleException()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var costs = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);

        // Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new ElectricityTariff(period, costs, Guid.Empty));
    }

    [Fact]
    public void UpdateCosts_WithValidCosts_UpdatesAndChangesDateUpdated()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var originalCosts = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);
        var companyId = Guid.NewGuid();

        var tariff = new ElectricityTariff(period, originalCosts, companyId);
        var originalDateUpdated = tariff.DateUpdated;

        // Esperar un poco para asegurar que DateUpdated sea diferente
        System.Threading.Thread.Sleep(10);

        var newCosts = new TariffCosts(825.50m, 380.00m, 56.03m, 290.00m, 25.00m, 75.00m, 3.71m, 0m, 9.813m);

        // Act
        tariff.UpdateCosts(newCosts);

        // Assert
        Assert.Equal(newCosts, tariff.Costs);
        Assert.True(tariff.DateUpdated > originalDateUpdated);
    }

    [Fact]
    public void UpdateCosts_WithNullCosts_ThrowsDomainRuleException()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var costs = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);
        var companyId = Guid.NewGuid();

        var tariff = new ElectricityTariff(period, costs, companyId);

        // Act & Assert
        Assert.Throws<DomainRuleException>(() => tariff.UpdateCosts(null!));
    }

    [Fact]
    public void GetTotalCosts_CalculatesCorrectly()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var costs = new TariffCosts(
            100m,  // TotalCu
            40m,   // PurchaseCostG
            30m,   // ChargeTransportStnTm
            20m,   // ChargeTransportSdlDm
            10m,   // MarketingMargin
            0m,    // CostLossesPr
            0m,    // RestrictionsRm
            0m,    // Cot
            0m);   // CfmjGfact

        var companyId = Guid.NewGuid();
        var tariff = new ElectricityTariff(period, costs, companyId);

        // Act
        var total = tariff.GetTotalCosts();

        // Assert
        Assert.Equal(100m + 40m + 30m + 20m + 10m, total);
    }

    [Fact]
    public void GetTotalCosts_WithNullComponents_CalculatesWithZero()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var costs = new TariffCosts(
            100m,   // TotalCu
            null,   // PurchaseCostG (null)
            null,   // ChargeTransportStnTm (null)
            20m,    // ChargeTransportSdlDm
            null,   // MarketingMargin (null)
            0m,
            0m,
            0m,
            0m);

        var companyId = Guid.NewGuid();
        var tariff = new ElectricityTariff(period, costs, companyId);

        // Act
        var total = tariff.GetTotalCosts();

        // Assert
        Assert.Equal(120m, total); // 100 + 20 (los nulls se tratan como 0)
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var costs = new TariffCosts(810.46m, 372.46m, 56.03m, 280.91m, 24.86m, 72.49m, 3.71m, 0m, 9.813m);
        var companyId = Guid.NewGuid();

        var tariff = new ElectricityTariff(period, costs, companyId);

        // Act
        var result = tariff.ToString();

        // Assert
        Assert.NotNull(result);
        Assert.Contains("2025", result);
        Assert.Contains("Enero", result);
        Assert.Contains("810.46", result); // TotalCu
    }
}
