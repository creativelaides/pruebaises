using System;
using System.Threading.Tasks;
using Mapster;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Handler para GetTariffByPeriodQuery.
/// WolverineFx lo descubre automáticamente.
/// 
/// Responsabilidad:
/// - Obtener tarifa por año y período
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
            // ✅ CORREGIDO: Pasar Period (string), no Month (int)
            var tariff = await _tariffs
                .GetByPeriodAsync(query.Year, query.Period);

            if (tariff == null)
                throw new ApplicationCaseException(
                    $"Tarifa no encontrada para el período {query.Year}-{query.Period}");

            return tariff.Adapt<GetTariffByPeriodResponse>();
        }, "Error al obtener la tarifa");
    }
}
