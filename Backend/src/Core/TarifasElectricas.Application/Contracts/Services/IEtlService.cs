using System;
using System.Threading.Tasks;

namespace TarifasElectricas.Application.Contracts.Services;

/// <summary>
/// Contrato para servicio de ETL (Extract, Transform, Load).
/// Extrae datos de Gov.co, transforma y carga usando Application handlers.
/// </summary>
public interface IEtlService
{
    /// <summary>
    /// Ejecuta el proceso ETL completo.
    /// </summary>
    Task<EtlExecutionResult> ExecuteAsync();
}

/// <summary>
/// Resultado de una ejecución ETL.
/// </summary>
public class EtlExecutionResult
{
    public bool Success { get; set; }
    public int ProcessedCount { get; set; }
    public int ErrorCount { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal DurationSeconds { get; set; }
    public DateTime ExecutionDate { get; set; } = DateTime.UtcNow;
}