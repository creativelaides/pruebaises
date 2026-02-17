using System;

namespace TarifasElectricas.Domain.Entities.Root;

/// <summary>
/// Clase base para todas las entidades del dominio.
/// Proporciona identificación única con Guid v7 y auditoría básica.
/// </summary>
public abstract class RootEntity
{
    /// <summary>
    /// Identificador único usando Guid v7 (sortable).
    /// </summary>
    public Guid Id { get; internal set; }

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización.
    /// </summary>
    public DateTime DateUpdated { get; internal set; } = DateTime.UtcNow;
}
