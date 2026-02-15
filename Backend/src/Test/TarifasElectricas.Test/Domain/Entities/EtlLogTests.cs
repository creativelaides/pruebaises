using System;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Enums;

namespace TarifasElectricas.Test.Domain.Entities;

public class EtlLogTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesEtlLog()
    {
        // Act
        var etlLog = new EtlLog(DateTime.UtcNow, EtlState.Success, 1500, "Completado", 45.32m);

        // Assert
        Assert.NotEqual(Guid.Empty, etlLog.Id);
        Assert.Equal(EtlState.Success, etlLog.State);
        Assert.Equal(1500, etlLog.ProcessedRecords);
    }

    [Fact]
    public void IsSuccess_WithSuccessState_ReturnsTrue()
    {
        // Arrange
        var etlLog = new EtlLog(DateTime.UtcNow, EtlState.Success, 1000, "OK", 30m);

        // Act & Assert
        Assert.True(etlLog.IsSuccess);
        Assert.True(etlLog.IsCompleted);
    }

    [Fact]
    public void HasIssues_WithFailedState_ReturnsTrue()
    {
        // Arrange
        var etlLog = new EtlLog(DateTime.UtcNow, EtlState.Failed, 500, "Error", 15m);

        // Act & Assert
        Assert.True(etlLog.HasIssues);
        Assert.False(etlLog.IsSuccess);
    }

}
