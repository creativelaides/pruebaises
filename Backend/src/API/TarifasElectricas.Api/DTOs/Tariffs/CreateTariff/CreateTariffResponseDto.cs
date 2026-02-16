namespace TarifasElectricas.Api.DTOs.Tariffs.CreateTariff;

public record CreateTariffResponseDto(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime CreatedAt);
