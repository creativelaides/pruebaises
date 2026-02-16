using System.Text.Json;
using TarifasElectricas.Infrastructure.Utils.Json;

namespace TarifasElectricas.Infrastructure.Utils.Mapping;

/// <summary>
/// Mapea filas crudas de Socrata a un modelo tipado.
/// </summary>
public static class SocrataRowMapper
{
    public static bool TryMapRow(JsonElement row, SocrataFieldMap fields, out TariffRow mapped)
    {
        mapped = default;

        var year = JsonFieldReader.ReadInt(row, fields.Year);
        var period = JsonFieldReader.ReadString(row, fields.Period);
        var level = JsonFieldReader.ReadString(row, fields.Level);
        var tariffOperator = JsonFieldReader.ReadString(row, fields.TariffOperator);

        if (year is null ||
            string.IsNullOrWhiteSpace(period) ||
            string.IsNullOrWhiteSpace(level) ||
            string.IsNullOrWhiteSpace(tariffOperator))
            return false;

        mapped = new TariffRow(
            year.Value,
            period.Trim(),
            level.Trim(),
            tariffOperator.Trim(),
            JsonFieldReader.ReadDecimal(row, fields.TotalCu),
            JsonFieldReader.ReadDecimal(row, fields.PurchaseCostG),
            JsonFieldReader.ReadDecimal(row, fields.ChargeTransportStnTm),
            JsonFieldReader.ReadDecimal(row, fields.ChargeTransportSdlDm),
            JsonFieldReader.ReadDecimal(row, fields.MarketingMargin),
            JsonFieldReader.ReadDecimal(row, fields.CostLossesPr),
            JsonFieldReader.ReadDecimal(row, fields.RestrictionsRm),
            JsonFieldReader.ReadDecimal(row, fields.Cot),
            JsonFieldReader.ReadDecimal(row, fields.CfmjGfact));

        return true;
    }
}
