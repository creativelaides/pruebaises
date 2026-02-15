using System;

namespace TarifasElectricas.Application.Exceptions;

/// <summary>
/// Excepción lanzada cuando ocurre un error en los casos de uso de la aplicación
/// </summary>
public class ApplicationCaseException : Exception
{
    public ApplicationCaseException(string message) : base(message)
    {
    }

    public ApplicationCaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}