namespace TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Value Object que representa el período y clasificación de la tarifa
/// </summary>
public record class TariffPeriod
(
    int Year,
    int Month,
    string? Period,
    string? Level,
    string? Operator
);
