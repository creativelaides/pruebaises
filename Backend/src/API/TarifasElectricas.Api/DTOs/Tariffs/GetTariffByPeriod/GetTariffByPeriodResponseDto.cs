namespace TarifasElectricas.Api.DTOs.Tariffs.GetTariffByPeriod;

public record GetTariffByPeriodResponseDto(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime CreatedAt);
