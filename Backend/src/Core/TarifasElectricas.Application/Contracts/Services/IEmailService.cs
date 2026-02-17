namespace TarifasElectricas.Application.Contracts.Services;

/// <summary>
/// Contrato para envío de correos electrónicos.
/// </summary>
public interface IEmailService
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}

/// <summary>
/// Mensaje de correo básico.
/// </summary>
public sealed record EmailMessage(
    string To,
    string Subject,
    string Body,
    bool IsHtml = false);
