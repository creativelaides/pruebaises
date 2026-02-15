using System;
using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Value Object que representa el período y clasificación de la tarifa
/// </summary>
public record TariffPeriod
{
    public int Year { get; init; }
    public int Month { get; init; }
    public string? Period { get; init; }
    public string? Level { get; init; }
    public string? TariffOperator { get; init; }

    public TariffPeriod(int year, int month, string? period, string? level, string? tariffOperator)
    {
        if (year < 1900 || year > DateTime.UtcNow.Year + 1)
        {
            throw new DomainRuleException($"Año inválido: {year}");
        }

        if (month < 1 || month > 12)
        {
            throw new DomainRuleException($"Mes inválido: {month}");
        }

        Year = year;
        Month = month;
        Period = period;
        Level = level;
        TariffOperator = tariffOperator;
    }
}
