using System;

namespace TarifasElectricas.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando se viola una regla de negocio del dominio
/// </summary>
public class DomainRuleException : Exception
{
    public DomainRuleException(string message) : base(message)
    {
    }

    public DomainRuleException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}