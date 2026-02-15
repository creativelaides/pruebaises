using System;
using TarifasElectricas.Domain.Entities.EntityRoot;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Domain.Entities;

public class EducationComponent : Root
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? SimpleDescription { get; set; }
    public string? Analogy { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int? Order { get; set; }

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private EducationComponent() { }

    /// <summary>
    /// Constructor público para crear una nuevo componente educativo
    /// </summary>
    public EducationComponent
    (
    string? code,
    string? name,
    string? simpleDescription,
    string? analogy,
    string? icon,
    string? color,
    int? order
    )
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainRuleException("El código no puede estar vacío");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre es requerido");

        Id = Guid.CreateVersion7();
        Code = code;
        Name = name;
        SimpleDescription = simpleDescription;
        Analogy = analogy;
        Icon = icon;
        Color = color;
        Order = order;
    }
}