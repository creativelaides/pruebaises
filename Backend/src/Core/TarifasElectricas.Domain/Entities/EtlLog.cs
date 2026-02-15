using System;
using TarifasElectricas.Domain.Entities.EntityRoot;
using TarifasElectricas.Domain.Enums;

namespace TarifasElectricas.Domain.Entities;

public class EtlLog : Root
{
    public DateTime ExecutionDate { get; set; }
    public EtlState State { get; set; }
    public int? ProcessedRecords { get; set; }
    public string? Message { get; set; }
    public decimal? DurationSeconds { get; set; }

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private EtlLog() { }

    /// <summary>
    /// Constructor público para crear un nuevo log de ETL
    /// </summary>
    public EtlLog(
    DateTime executionDate,
    EtlState state,
    int? processedRecords = null,
    string? message = null,
    decimal? durationSeconds = null
    )
    {
        Id = Guid.CreateVersion7();
        ExecutionDate = executionDate;
        State = state;
        ProcessedRecords = processedRecords;
        Message = message;
        DurationSeconds = durationSeconds;
    }

    /// <summary>
    /// Indica si el proceso fue exitoso
    /// </summary>
    public bool IsSuccess => State == EtlState.Success;

    /// <summary>
    /// Indica si el proceso finalizó (exitoso, fallido o cancelado)
    /// </summary>
    public bool IsCompleted => State != EtlState.Running;

    /// <summary>
    /// Indica si el proceso tuvo algún problema (falló o fue cancelado)
    /// </summary>
    public bool HasIssues => State == EtlState.Failed || State == EtlState.Cancelled;

    public override string ToString() =>
        $"[{ExecutionDate:yyyy-MM-dd HH:mm:ss}] {State} | Registros: {ProcessedRecords} | Duración: {DurationSeconds}s";
}