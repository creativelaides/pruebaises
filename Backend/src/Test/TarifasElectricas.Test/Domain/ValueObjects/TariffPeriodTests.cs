using System;
using Xunit;
using TarifasElectricas.Domain.ValueObjects;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Test.Domain.ValueObjects;

/// <summary>
/// Tests para TariffPeriod Value Object
/// Valida: creación, validaciones, formato
/// </summary>
public class TariffPeriodTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesTariffPeriod()
    {
        // Arrange & Act
        var period = new TariffPeriod(
            2025,
            "Enero",
            "Nivel 1 (Propiedad OR)",
            "ENEL Bogotá - Cundinamarca",
            2025);

        // Assert
        Assert.NotNull(period);
        Assert.Equal(2025, period.Year);
        Assert.Equal("Enero", period.Period);
        Assert.Equal("Nivel 1 (Propiedad OR)", period.Level);
        Assert.Equal("ENEL Bogotá - Cundinamarca", period.TariffOperator);
    }

    [Fact]
    public void Constructor_WithYearTooOld_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(1899, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithYearTooNew_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        var maxYear = 2025 + 1;
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(maxYear + 1, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithValidBoundaryYears_CreatesSuccessfully()
    {
        // Arrange & Act
        var minYear = new TariffPeriod(1900, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var maxYear = new TariffPeriod(2026, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);

        // Assert
        Assert.Equal(1900, minYear.Year);
        Assert.Equal(2026, maxYear.Year);
    }

    [Fact]
    public void Constructor_WithEmptyPeriod_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, "", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithNullPeriod_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, null!, "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithPeriodTooLong_ThrowsDomainRuleException()
    {
        // Arrange
        var longPeriod = new string('A', 101); // 101 caracteres

        // Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, longPeriod, "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithEmptyLevel_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, "Enero", "", "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithNullLevel_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, "Enero", null!, "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithLevelTooLong_ThrowsDomainRuleException()
    {
        // Arrange
        var longLevel = new string('B', 101); // 101 caracteres

        // Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, "Enero", longLevel, "ENEL Bogotá - Cundinamarca", 2025));
    }

    [Fact]
    public void Constructor_WithEmptyTariffOperator_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "", 2025));
    }

    [Fact]
    public void Constructor_WithNullTariffOperator_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", null!, 2025));
    }

    [Fact]
    public void Constructor_WithTariffOperatorTooLong_ThrowsDomainRuleException()
    {
        // Arrange
        var longOperator = new string('C', 301); // 301 caracteres

        // Act & Assert
        Assert.Throws<DomainRuleException>(() =>
            new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", longOperator, 2025));
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var period = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);

        // Act
        var result = period.ToString();

        // Assert
        Assert.NotNull(result);
        Assert.Contains("2025", result);
        Assert.Contains("Enero", result);
        Assert.Contains("Nivel 1 (Propiedad OR)", result);
        Assert.Contains("ENEL Bogotá - Cundinamarca", result);
    }

    [Fact]
    public void Equality_WithSameValues_AreEqual()
    {
        // Arrange
        var period1 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var period2 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);

        // Act & Assert
        Assert.Equal(period1, period2);
    }

    [Fact]
    public void Equality_WithDifferentYear_AreNotEqual()
    {
        // Arrange
        var period1 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var period2 = new TariffPeriod(2024, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);

        // Act & Assert
        Assert.NotEqual(period1, period2);
    }

    [Fact]
    public void Equality_WithDifferentPeriod_AreNotEqual()
    {
        // Arrange
        var period1 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var period2 = new TariffPeriod(2025, "Febrero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);

        // Act & Assert
        Assert.NotEqual(period1, period2);
    }

    [Fact]
    public void Equality_WithDifferentTariffOperator_AreNotEqual()
    {
        // Arrange
        var period1 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var period2 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "CELSIA Colombia - Valle del Cauca", 2025);

        // Act & Assert
        Assert.NotEqual(period1, period2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ProduceSameHash()
    {
        // Arrange
        var period1 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);
        var period2 = new TariffPeriod(2025, "Enero", "Nivel 1 (Propiedad OR)", "ENEL Bogotá - Cundinamarca", 2025);

        // Act
        var hash1 = period1.GetHashCode();
        var hash2 = period2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }
}
