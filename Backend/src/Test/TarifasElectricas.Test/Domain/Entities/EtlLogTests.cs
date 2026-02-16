using System;
using Xunit;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Enums;

namespace TarifasElectricas.Test.Domain.Entities;

/// <summary>
/// Tests para EtlLog Entity
/// Valida: creación, propiedades computadas, formato
/// </summary>
public class EtlLogTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesEtlLog()
    {
        // Arrange
        var executionDate = DateTime.UtcNow;
        var message = "ETL completado exitosamente";

        // Act
        var log = new EtlLog(executionDate, EtlState.Success, 100, message, 45.5m);

        // Assert
        Assert.NotNull(log);
        Assert.NotEqual(Guid.Empty, log.Id);
        Assert.Equal(executionDate, log.ExecutionDate);
        Assert.Equal(EtlState.Success, log.State);
        Assert.Equal(100, log.ProcessedRecords);
        Assert.Equal(message, log.Message);
        Assert.Equal(45.5m, log.DurationSeconds);
    }

    [Fact]
    public void Constructor_WithMinimalData_CreatesEtlLog()
    {
        // Arrange
        var executionDate = DateTime.UtcNow;

        // Act
        var log = new EtlLog(executionDate, EtlState.Running);

        // Assert
        Assert.NotNull(log);
        Assert.Equal(executionDate, log.ExecutionDate);
        Assert.Equal(EtlState.Running, log.State);
        Assert.Null(log.ProcessedRecords);
        Assert.Null(log.Message);
        Assert.Null(log.DurationSeconds);
    }

    [Fact]
    public void IsSuccess_WithSuccessState_ReturnsTrue()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Success, 100, "OK", 10m);

        // Act
        var result = log.IsSuccess;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSuccess_WithFailedState_ReturnsFalse()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Failed, null, "Error", 5m);

        // Act
        var result = log.IsSuccess;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCompleted_WithRunningState_ReturnsFalse()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Running);

        // Act
        var result = log.IsCompleted;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCompleted_WithSuccessState_ReturnsTrue()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Success, 100, "OK", 10m);

        // Act
        var result = log.IsCompleted;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsCompleted_WithFailedState_ReturnsTrue()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Failed, null, "Error", 5m);

        // Act
        var result = log.IsCompleted;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsCompleted_WithCancelledState_ReturnsTrue()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Cancelled, 50, "Cancelado", 3m);

        // Act
        var result = log.IsCompleted;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasIssues_WithFailedState_ReturnsTrue()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Failed, null, "Error", 5m);

        // Act
        var result = log.HasIssues;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasIssues_WithCancelledState_ReturnsTrue()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Cancelled, 50, "Cancelado", 3m);

        // Act
        var result = log.HasIssues;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasIssues_WithSuccessState_ReturnsFalse()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Success, 100, "OK", 10m);

        // Act
        var result = log.HasIssues;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasIssues_WithRunningState_ReturnsFalse()
    {
        // Arrange
        var log = new EtlLog(DateTime.UtcNow, EtlState.Running);

        // Act
        var result = log.HasIssues;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var executionDate = new DateTime(2025, 1, 15, 10, 30, 45);
        var log = new EtlLog(executionDate, EtlState.Success, 150, null, 30.5m);

        // Act
        var result = log.ToString();

        // Assert
        Assert.NotNull(result);
        Assert.Contains("2025-01-15", result);
        Assert.Contains("Success", result);
        Assert.Contains("150", result);
        Assert.Contains("30.5", result);
    }

    [Theory]
    [InlineData(EtlState.Success)]
    [InlineData(EtlState.Failed)]
    [InlineData(EtlState.Running)]
    [InlineData(EtlState.Cancelled)]
    public void Constructor_WithAllStates_CreatesSuccessfully(EtlState state)
    {
        // Arrange & Act
        var log = new EtlLog(DateTime.UtcNow, state, 0, "Test", 1m);

        // Assert
        Assert.NotNull(log);
        Assert.Equal(state, log.State);
    }
}