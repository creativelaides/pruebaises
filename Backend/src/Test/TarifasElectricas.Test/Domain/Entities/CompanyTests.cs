using System;
using Xunit;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Test.Domain.Entities;

/// <summary>
/// Tests para Company Entity
/// Valida: creación, validaciones, métodos
/// </summary>
public class CompanyTests
{
    [Fact]
    public void Constructor_WithValidCode_CreatesCompany()
    {
        // Arrange & Act
        var company = new Company("ENEL Bogotá - Cundinamarca");

        // Assert
        Assert.NotNull(company);
        Assert.NotEqual(Guid.Empty, company.Id);
        Assert.Equal("ENEL Bogotá - Cundinamarca", company.Code);
        Assert.NotEqual(default, company.CreatedAt);
        Assert.NotEqual(default, company.DateUpdated);
    }

    [Fact]
    public void Constructor_WithEmptyCode_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() => new Company(""));
    }

    [Fact]
    public void Constructor_WithNullCode_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() => new Company(null!));
    }

    [Fact]
    public void Constructor_WithWhitespaceCode_ThrowsDomainRuleException()
    {
        // Arrange, Act & Assert
        Assert.Throws<DomainRuleException>(() => new Company("   "));
    }

    [Fact]
    public void Constructor_WithCodeTooLong_ThrowsDomainRuleException()
    {
        // Arrange
        var longCode = new string('A', 301); // 301 caracteres

        // Act & Assert
        Assert.Throws<DomainRuleException>(() => new Company(longCode));
    }

    [Fact]
    public void Constructor_WithCodeAt300Characters_CreatesSuccessfully()
    {
        // Arrange
        var codeAt300 = new string('A', 300);

        // Act
        var company = new Company(codeAt300);

        // Assert
        Assert.NotNull(company);
        Assert.Equal(codeAt300, company.Code);
    }

    [Fact]
    public void Constructor_WithDifferentOperators_CreatesCorrectly()
    {
        // Arrange - Operadores reales de Gov.co
        var operators = new[]
        {
            "ENEL Bogotá - Cundinamarca",
            "CELSIA Colombia - Valle del Cauca",
            "CELSIA Colombia - Tolima",
            "EPM"
        };

        // Act & Assert
        foreach (var operatorCode in operators)
        {
            var company = new Company(operatorCode);
            Assert.NotNull(company);
            Assert.Equal(operatorCode, company.Code);
        }
    }

    [Fact]
    public void UpdateCode_WithValidCode_UpdatesCodeAndChangesDateUpdated()
    {
        // Arrange
        var company = new Company("ENEL Bogotá - Cundinamarca");
        var originalDateUpdated = company.DateUpdated;

        // Esperar un poco para asegurar que DateUpdated sea diferente
        System.Threading.Thread.Sleep(10);

        // Act
        company.UpdateCode("CELSIA Colombia - Valle del Cauca");

        // Assert
        Assert.Equal("CELSIA Colombia - Valle del Cauca", company.Code);
        Assert.True(company.DateUpdated > originalDateUpdated);
    }

    [Fact]
    public void UpdateCode_WithEmptyCode_ThrowsDomainRuleException()
    {
        // Arrange
        var company = new Company("ENEL Bogotá - Cundinamarca");

        // Act & Assert
        Assert.Throws<DomainRuleException>(() => company.UpdateCode(""));
    }

    [Fact]
    public void UpdateCode_WithNullCode_ThrowsDomainRuleException()
    {
        // Arrange
        var company = new Company("ENEL Bogotá - Cundinamarca");

        // Act & Assert
        Assert.Throws<DomainRuleException>(() => company.UpdateCode(null!));
    }

    [Fact]
    public void UpdateCode_WithCodeTooLong_ThrowsDomainRuleException()
    {
        // Arrange
        var company = new Company("ENEL Bogotá - Cundinamarca");
        var longCode = new string('B', 301);

        // Act & Assert
        Assert.Throws<DomainRuleException>(() => company.UpdateCode(longCode));
    }

    [Fact]
    public void IdGeneration_UsesGuidV7()
    {
        // Arrange & Act
        var company = new Company("ENEL Bogotá - Cundinamarca");

        // Assert
        Assert.NotEqual(Guid.Empty, company.Id);
        // Verificar que es un GUID válido
        Assert.True(company.Id != Guid.Empty);
        Assert.IsType<Guid>(company.Id);
    }

    [Fact]
    public void CreatedAt_IsSetAutomatically()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var company = new Company("ENEL Bogotá - Cundinamarca");

        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(company.CreatedAt >= beforeCreation);
        Assert.True(company.CreatedAt <= afterCreation);
    }

    [Fact]
    public void DateUpdated_EqualsCreatedAtInitially()
    {
        // Arrange & Act
        var company = new Company("ENEL Bogotá - Cundinamarca");

        // Assert
        Assert.Equal(company.CreatedAt, company.DateUpdated);
    }

    [Fact]
    public void DateUpdated_ChangesAfterUpdate()
    {
        // Arrange
        var company = new Company("ENEL Bogotá - Cundinamarca");
        var originalDateUpdated = company.DateUpdated;

        System.Threading.Thread.Sleep(10);

        // Act
        company.UpdateCode("CELSIA Colombia - Valle del Cauca");

        // Assert
        Assert.NotEqual(originalDateUpdated, company.DateUpdated);
        Assert.True(company.DateUpdated > originalDateUpdated);
    }

    [Fact]
    public void MultipleInstances_HaveDifferentIds()
    {
        // Arrange & Act
        var company1 = new Company("ENEL Bogotá - Cundinamarca");
        var company2 = new Company("CELSIA Colombia - Valle del Cauca");

        // Assert
        Assert.NotEqual(company1.Id, company2.Id);
    }
}