using TarifasElectricas.Domain.Exceptions;
namespace TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Value Object que representa un componente individual de una factura simulada.
/// 
/// Encapsula: nombre del componente, valor monetario, y explicación educativa
/// 
/// Ejemplos:
/// - InvoiceComponent("Consumo de Energía", 30000, "Lo que pagas por la electricidad que consumiste")
/// - InvoiceComponent("Transporte", 15000, "Llevar la energía desde plantas generadoras a tu casa")
/// - InvoiceComponent("Comercialización", 5000, "Servicio de lectura y atención al cliente")
/// </summary>
public class InvoiceComponent
{
    /// <summary>
    /// Nombre del componente (ej: "Consumo de Energía")
    /// Validación: No vacío, máximo 100 caracteres
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Valor monetario del componente para un consumo específico
    /// Validación: No puede ser negativo
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// Explicación educativa del componente
    /// Se muestra al usuario para entender qué es este componente
    /// Validación: No vacío, máximo 500 caracteres
    /// </summary>
    public string Explanation { get; init; } = null!;

    /// <summary>
    /// Constructor con validaciones de negocio
    /// </summary>
    public InvoiceComponent(string name, decimal value, string explanation)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre del componente es requerido");

        if (value < 0)
            throw new DomainRuleException("El valor del componente no puede ser negativo");

        if (string.IsNullOrWhiteSpace(explanation))
            throw new DomainRuleException("La explicación del componente es requerida");

        if (name.Length > 100)
            throw new DomainRuleException("El nombre no puede exceder 100 caracteres");

        if (explanation.Length > 500)
            throw new DomainRuleException("La explicación no puede exceder 500 caracteres");

        Name = name;
        Value = value;
        Explanation = explanation;
    }

    /// <summary>
    /// Igualdad por valor
    /// </summary>
    public override bool Equals(object? obj) =>
        obj is InvoiceComponent other &&
        Name == other.Name &&
        Value == other.Value &&
        Explanation == other.Explanation;

    /// <summary>
    /// Hash code basado en todas las propiedades
    /// </summary>
    public override int GetHashCode() =>
        HashCode.Combine(Name, Value, Explanation);

    /// <summary>
    /// Representación legible
    /// Ej: "Consumo de Energía: 30000"
    /// </summary>
    public override string ToString() =>
        $"{Name}: {Value}";
}