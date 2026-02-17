using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using TarifasElectricas.Application.Contracts.Services;
using TarifasElectricas.Infrastructure.Options;

namespace TarifasElectricas.Infrastructure.Services;

/// <summary>
/// Implementación SMTP para envío de correos.
/// </summary>
public sealed class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _options;

    public SmtpEmailService(IOptions<EmailOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        using var smtpClient = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.UseSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_options.User, _options.Password)
        };

        var fromAddress = new MailAddress(
            string.IsNullOrWhiteSpace(_options.FromEmail) ? _options.User : _options.FromEmail,
            _options.FromName);

        using var mailMessage = new MailMessage
        {
            From = fromAddress,
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.IsHtml
        };

        mailMessage.To.Add(message.To);

        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }
}
