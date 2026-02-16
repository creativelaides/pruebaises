using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Infrastructure.Utils.Mapping;

/// <summary>
/// Llave de comparación para identificar tarifas únicas por período y operador.
/// </summary>
public sealed record TariffKey(
    int Year,
    string Period,
    string Level,
    string TariffOperator)
{
    public static TariffKey From(TariffRow row) =>
        new(row.Year, Normalize(row.Period), Normalize(row.Level), Normalize(row.TariffOperator));

    public static TariffKey From(TariffPeriod period) =>
        new(period.Year, Normalize(period.Period), Normalize(period.Level), Normalize(period.TariffOperator));

    private static string Normalize(string value) =>
        value.Trim().ToUpperInvariant();
}
