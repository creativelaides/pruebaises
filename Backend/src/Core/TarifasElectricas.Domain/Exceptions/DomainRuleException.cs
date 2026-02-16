using System;

namespace TarifasElectricas.Domain.Exceptions;

/// <summary>
/// Excepción que representa la violación de una regla de negocio en el dominio.
/// Se lanza cuando la entrada no cumple con las validaciones de negocio.
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