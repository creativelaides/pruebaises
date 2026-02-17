namespace TarifasElectricas.Infrastructure.Options;

/// <summary>
/// Opciones de configuración para SMTP.
/// </summary>
public sealed class EmailOptions
{
    public const string SectionName = "Email";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "PruebaIses";
    public bool UseSsl { get; set; } = true;
}
