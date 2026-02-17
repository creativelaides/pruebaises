using System;

namespace TarifasElectricas.Domain.Entities.Root;

/// <summary>
/// Entidad con auditoría de usuario.
/// </summary>
public abstract class AuditableEntity : RootEntity
{
    /// <summary>
    /// Usuario que creó el registro.
    /// </summary>
    public string? CreatedBy { get; internal set; }

    /// <summary>
    /// Usuario que actualizó el registro por última vez.
    /// </summary>
    public string? UpdatedBy { get; internal set; }
}
