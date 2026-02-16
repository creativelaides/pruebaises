using System;
using Mapster;
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
                .Select(t => t.Adapt<GetAllTariffsResponse.TariffItem>())
                .ToList();

            return new GetAllTariffsResponse(tariffItems);
        }, "Error al obtener las tarifas");
    }
}

