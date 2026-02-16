namespace TarifasElectricas.Api.DTOs.Tariffs.GetAllTariffs;

public record GetAllTariffsResponseDto(
    IEnumerable<GetAllTariffsResponseDto.TariffItem> Tariffs)
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
