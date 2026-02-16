namespace TarifasElectricas.Api.DTOs.Etl.ExecuteEtl;

public record ExecuteEtlResponseDto(
    bool Success,
    int ProcessedCount,
    int ErrorCount,
    string Message,
    decimal DurationSeconds,
    DateTime ExecutionDate);
