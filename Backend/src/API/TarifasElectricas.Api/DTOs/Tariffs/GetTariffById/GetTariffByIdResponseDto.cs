namespace TarifasElectricas.Api.DTOs.Tariffs.GetTariffById;

public record GetTariffByIdResponseDto(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime CreatedAt);
