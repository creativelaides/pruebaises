using System.Globalization;
using System.Text.Json;

namespace TarifasElectricas.Infrastructure.Utils.Json;

/// <summary>
/// Lecturas seguras de valores JSON para filas Socrata.
/// </summary>
public static class JsonFieldReader
{
    public static string? ReadString(JsonElement row, string field)
    {
        if (!row.TryGetProperty(field, out var value))
            return null;

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.GetRawText(),
            JsonValueKind.Null => null,
            JsonValueKind.Undefined => null,
            _ => value.ToString()
        };
    }

    public static int? ReadInt(JsonElement row, string field)
    {
        if (!row.TryGetProperty(field, out var value))
            return null;

        if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var intValue))
            return intValue;

        var text = ReadString(row, field);
        if (string.IsNullOrWhiteSpace(text))
            return null;

        text = text.Replace(",", string.Empty).Trim();
        return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;
    }

    public static decimal? ReadDecimal(JsonElement row, string field)
    {
        if (!row.TryGetProperty(field, out var value))
            return null;

        if (value.ValueKind == JsonValueKind.Number && value.TryGetDecimal(out var decimalValue))
            return decimalValue;

        var text = ReadString(row, field);
        if (string.IsNullOrWhiteSpace(text))
            return null;

        text = text.Replace(",", string.Empty).Trim();
        return decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;
    }
}
