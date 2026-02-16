using System;
namespace TarifasElectricas.Domain.Entities.EntityRoot;

/// <summary>
/// Clase base para todas las entidades del dominio.
/// Proporciona identificación única con Guid v7 y auditoría básica.
/// </summary>
public abstract class Root
{
    /// <summary>
    /// Identificador único usando Guid v7 (sortable).
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Fecha de creación del registro
    /// Establecido automáticamente al crear la entidad.
    /// </summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización.
    /// Actualizado cada vez que se modifica la entidad.
    /// </summary>
    public DateTime DateUpdated { get; protected set; } = DateTime.UtcNow;
}