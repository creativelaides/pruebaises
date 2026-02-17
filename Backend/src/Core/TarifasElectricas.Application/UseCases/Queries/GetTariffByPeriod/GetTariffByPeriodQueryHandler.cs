using System;
using System.Linq;
using System.Threading.Tasks;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Handler para GetTariffByPeriodQuery.
/// WolverineFx lo descubre automáticamente.
/// 
/// Responsabilidad:
/// - Obtener tarifas por filtros opcionales
/// - Mapear a response
/// - Manejo de errores
/// </summary>
public class GetTariffByPeriodQueryHandler(IElectricityTariffRepository tariffs)
{
    private readonly IElectricityTariffRepository _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));

    public async Task<GetTariffByPeriodResponse> Handle(GetTariffByPeriodQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            var tariffs = (await _tariffs
                .GetByFiltersAsync(
                    query.Year,
                    query.Period,
                    query.TariffOperator,
                    query.Level))
                .ToList();

            if (tariffs.Count == 0)
                throw new ApplicationCaseException(
                    "No hay tarifas con los filtros solicitados");

            return new GetTariffByPeriodResponse(
                tariffs.Select(t => new GetTariffByPeriodResponse.TariffItem(
                    t.Id,
                    t.Period.Year,
                    t.Period.Period,
                    t.Period.Level,
                    t.Period.TariffOperator,
                    t.CompanyId,
                    t.GetTotalCosts(),
                    t.CreatedAt)));
        }, "Error al obtener la tarifa");
    }
}
