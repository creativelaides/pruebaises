using System;
using TarifasElectricas.Domain.Entities.Root;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Domain.Entities;

/// <summary>
/// Entidad que representa una empresa distribuidora de energía eléctrica.
/// 
/// Origen: Viene del campo "operador_de_red" del dataset de Gov.co
/// Ejemplos: "ENEL Bogotá - Cundinamarca", "CELSIA Colombia - Valle del Cauca"
/// 
/// Responsabilidad: Identificar de forma única el operador distribuidor
/// de energía en el mercado regulado.
/// </summary>
public class Company : AuditableEntity
{
    /// <summary>
    /// Código o nombre único del operador de red (exactamente como viene de Gov.co)
    /// 
    /// Ejemplos:
    /// - "ENEL Bogotá - Cundinamarca"
    /// - "CELSIA Colombia - Valle del Cauca"
    /// - "CELSIA Colombia - Tolima"
    /// 
    /// Máximo 300 caracteres (según datos reales de Gov.co)
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    /// Constructor privado requerido por EF Core
    /// </summary>
    private Company() { }

    /// <summary>
    /// Constructor público para crear una nueva empresa.
    /// 
    /// Parámetros:
    ///   code: Código/nombre del operador tal como aparece en Gov.co
    /// 
    /// Validaciones:
    ///   - Code no puede estar vacío
    ///   - Code no puede exceder 300 caracteres
    /// </summary>
    public Company(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainRuleException("El código del operador es requerido");

        if (code.Length > 300)
            throw new DomainRuleException("El código del operador no puede exceder 300 caracteres");

        Id = Guid.CreateVersion7();
        Code = code;
        var now = DateTime.UtcNow;
        CreatedAt = now;
        DateUpdated = now;
    }

    /// <summary>
    /// Actualiza el código del operador (si es necesario corregir datos)
    /// </summary>
    public void UpdateCode(string newCode)
    {
        if (string.IsNullOrWhiteSpace(newCode))
            throw new DomainRuleException("El código del operador es requerido");

        if (newCode.Length > 300)
            throw new DomainRuleException("El código del operador no puede exceder 300 caracteres");

        Code = newCode;
        DateUpdated = DateTime.UtcNow;
    }
}
