using TarifasElectricas.Infrastructure.Utils.Mapping;

namespace TarifasElectricas.Infrastructure.Options;

/// <summary>
/// Opciones de configuración para consumir el dataset de Socrata (datos.gov.co).
/// </summary>
public sealed class SocrataOptions
{
    public const string SectionName = "Socrata";

    public string BaseUrl { get; set; } = "https://www.datos.gov.co";
    public string DatasetId { get; set; } = "ytme-6qnu";
    public string? AppToken { get; set; }
    public int PageSize { get; set; } = 1000;
    public int TimeoutSeconds { get; set; } = 30;

    public SocrataFieldMap Fields { get; set; } = new();
}
