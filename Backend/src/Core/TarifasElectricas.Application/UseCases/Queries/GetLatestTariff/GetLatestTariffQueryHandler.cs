using System;
using System.Threading.Tasks;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;

/// <summary>
/// Handler para GetLatestTariffQuery.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class GetLatestTariffQueryHandler(IElectricityTariffRepository tariffs)
{
    private readonly IElectricityTariffRepository _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));

    public async Task<GetLatestTariffResponse> Handle(GetLatestTariffQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            var tariff = await _tariffs.GetLatestAsync();

            if (tariff == null)
                throw new ApplicationCaseException("No hay tarifas disponibles");

            return new GetLatestTariffResponse(
                tariff.Id,
                tariff.Period.Year,
                tariff.Period.Period,
                tariff.Period.Level,
                tariff.Period.TariffOperator,
                tariff.CompanyId,
                tariff.GetTotalCosts(),
                tariff.CreatedAt);
        }, "Error al obtener la tarifa más reciente");
    }
}
