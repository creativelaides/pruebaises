using System.Collections.Generic;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Respuesta de la query GetTariffByPeriodQuery.
/// Record - Inmutable.
/// </summary>
public record GetTariffByPeriodResponse(IEnumerable<GetTariffByPeriodResponse.TariffItem> Tariffs)
{
    public record TariffItem(
        Guid Id,
        int Year,
        string? Period,
        string? Level,
        string? TariffOperator,
        Guid CompanyId,
        decimal TotalCosts,
        DateTime CreatedAt);
}
