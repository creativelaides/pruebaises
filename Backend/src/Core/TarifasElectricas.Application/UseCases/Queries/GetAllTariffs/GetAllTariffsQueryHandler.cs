using System;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;

/// <summary>
/// Handler para GetAllTariffsQuery.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class GetAllTariffsQueryHandler(IElectricityTariffRepository tariffs)
{
    private readonly IElectricityTariffRepository _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));

    public async Task<GetAllTariffsResponse> Handle(GetAllTariffsQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            var tariffs = await _tariffs.GetAllAsync();

            var skip = (query.Page - 1) * query.PageSize;
            var tariffItems = tariffs
                .OrderByDescending(t => t.CreatedAt)
                .Skip(skip)
                .Take(query.PageSize)
                .Select(t => new GetAllTariffsResponse.TariffItem(
                    t.Id,
                    t.Period.Year,
                    t.Period.Period,
                    t.Period.Level,
                    t.Period.TariffOperator,
                    t.CompanyId,
                    t.GetTotalCosts(),
                    t.CreatedAt))
                .ToList();

            return new GetAllTariffsResponse(tariffItems);
        }, "Error al obtener las tarifas");
    }
}

