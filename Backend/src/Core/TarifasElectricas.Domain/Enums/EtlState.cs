using System;
using TarifasElectricas.Domain.Entities.EntityRoot;

namespace TarifasElectricas.Domain.Enums;

/// <summary>
/// Estados posibles en el ciclo de vida de un proceso ETL.
/// </summary>
public enum EtlState
{
    /// <summary>
    /// El proceso está en ejecución.
    /// </summary>
    Running = 1,

    /// <summary>
    /// El proceso se completó exitosamente.
    /// </summary>
    Success = 2,

    /// <summary>
    /// El proceso falló durante la ejecución.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// El proceso fue cancelado.
    /// </summary>
    Cancelled = 4
}
