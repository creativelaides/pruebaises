using System;
using TarifasElectricas.Domain.Entities.Root;
using TarifasElectricas.Domain.Enums;

namespace TarifasElectricas.Domain.Entities;

/// <summary>
/// Entidad que registra la ejecución del proceso ETL
/// (Extract, Transform, Load de datos de Gov.co)
/// 
/// Responsabilidad: Auditar cada ejecución del ETL
/// - Cuándo se ejecutó
/// - Qué estado tuvo (Running, Success, Failed, Cancelled)
/// - Cuántos registros se procesaron
/// - Cuánto tiempo tomó
/// - Qué errores ocurrieron (si los hay)
/// </summary>
public class EtlLog : AuditableEntity
{
    public DateTime ExecutionDate { get; private set; }
    public EtlState State { get; private set; }
    public int? ProcessedRecords { get; private set; }
    public string? Message { get; private set; }
    public decimal? DurationSeconds { get; private set; }

    /// <summary>
    /// Constructor privado para EF Core.
    /// </summary>
    private EtlLog() { }

    /// <summary>
    /// Constructor público para crear un nuevo log de ETL.
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
    /// Indica si el proceso fue exitoso.
    /// </summary>
    public bool IsSuccess => State == EtlState.Success;

    /// <summary>
    /// Indica si el proceso finalizó (exitoso, fallido o cancelado).
    /// </summary>
    public bool IsCompleted => State != EtlState.Running;

    /// <summary>
    /// Indica si el proceso tuvo algún problema (falló o fue cancelado).
    /// </summary>
    public bool HasIssues => State == EtlState.Failed || State == EtlState.Cancelled;

    public override string ToString() =>
        FormattableString.Invariant(
            $"[{ExecutionDate:yyyy-MM-dd HH:mm:ss}] {State} | Registros: {ProcessedRecords} | Duración: {DurationSeconds}s");
}
