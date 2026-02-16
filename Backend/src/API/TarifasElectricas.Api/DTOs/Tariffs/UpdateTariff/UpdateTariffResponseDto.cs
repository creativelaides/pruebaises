namespace TarifasElectricas.Api.DTOs.Tariffs.UpdateTariff;

public record UpdateTariffResponseDto(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime UpdatedAt);
