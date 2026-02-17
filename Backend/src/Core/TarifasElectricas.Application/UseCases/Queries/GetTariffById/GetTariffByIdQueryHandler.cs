using System;
using System.Threading.Tasks;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffById;

/// <summary>
/// Handler para GetTariffByIdQuery.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class GetTariffByIdQueryHandler(IElectricityTariffRepository tariffs)
{
    private readonly IElectricityTariffRepository _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));

    public async Task<GetTariffByIdResponse> Handle(GetTariffByIdQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            var tariff = await _tariffs.GetByIdAsync(query.Id);

            if (tariff == null)
                throw new ApplicationCaseException($"Tarifa con ID {query.Id} no encontrada");

            return new GetTariffByIdResponse(
                tariff.Id,
                tariff.Period.Year,
                tariff.Period.Period,
                tariff.Period.Level,
                tariff.Period.TariffOperator,
                tariff.CompanyId,
                tariff.GetTotalCosts(),
                tariff.CreatedAt);
        }, "Error al obtener la tarifa");
    }
}
