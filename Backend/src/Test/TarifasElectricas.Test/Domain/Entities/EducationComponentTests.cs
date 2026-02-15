using System;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Test.Domain.Entities;

public class EducationComponentTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesEducationComponent()
    {
        // Act
        var component = new EducationComponent("EDU001", "Extract Phase", "Extraction", "Gathering", "📦", "#FF5733", 1);

        // Assert
        Assert.NotEqual(Guid.Empty, component.Id);
        Assert.Equal("EDU001", component.Code);
        Assert.Equal("Extract Phase", component.Name);
    }

    [Fact]
    public void Constructor_WithNullCode_ThrowsDomainRuleException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleException>(
            () => new EducationComponent(null, "Name", "Desc", "Analogy", "Icon", "Color", 1)
        );
        Assert.Contains("código", exception.Message.ToLower());
    }

    [Fact]
    public void Constructor_WithNullName_ThrowsDomainRuleException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleException>(
            () => new EducationComponent("EDU001", null, "Desc", "Analogy", "Icon", "Color", 1)
        );
        Assert.Contains("nombre", exception.Message.ToLower());
    }
}
