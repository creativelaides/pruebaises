namespace TarifasElectricas.Api.DTOs.Tariffs.DeleteTariff;

public record DeleteTariffResponseDto(
    Guid Id,
    bool Success,
    string Message);
