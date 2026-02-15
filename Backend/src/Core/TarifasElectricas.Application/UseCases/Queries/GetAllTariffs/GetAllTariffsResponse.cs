using static TarifasElectricas.Application.UseCases.Queries.GetAllTariffs.GetAllTariffsResponse;

namespace TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;

/// <summary>
/// Respuesta de la query GetAllTariffsQuery.
/// Record - Inmutable.
/// </summary>
public record GetAllTariffsResponse(
    IEnumerable<TariffItem> Tariffs)
{
    public record TariffItem(
        Guid Id,
        int Year,
        int Month,
        string? Period,
        string? Level,
        string? Operator,
        decimal TotalCosts,
        DateTime CreatedAt);
}
