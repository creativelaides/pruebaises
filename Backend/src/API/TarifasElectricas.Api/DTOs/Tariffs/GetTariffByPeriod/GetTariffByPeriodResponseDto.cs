using System;
using System.Collections.Generic;

namespace TarifasElectricas.Api.DTOs.Tariffs.GetTariffByPeriod;

public record GetTariffByPeriodResponseDto(IEnumerable<GetTariffByPeriodResponseDto.TariffItem> Tariffs)
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
