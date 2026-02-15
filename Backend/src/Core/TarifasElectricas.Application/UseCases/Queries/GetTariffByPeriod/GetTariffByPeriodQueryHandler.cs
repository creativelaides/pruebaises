using System;
using System.Threading.Tasks;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Handler para GetTariffByPeriodQuery.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class GetTariffByPeriodQueryHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<GetTariffByPeriodResponse> Handle(GetTariffByPeriodQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var tariff = await _unitOfWork.ElectricityTariffs
                .GetByPeriodAsync(query.Year, query.Month);

            if (tariff == null)
                throw new ApplicationCaseException(
                    $"Tarifa no encontrada para el período {query.Year}-{query.Month:D2}");

            return new GetTariffByPeriodResponse(
                tariff.Id,
                tariff.Period.Year,
                tariff.Period.Month,
                tariff.Period.Period,
                tariff.Period.Level,
                tariff.Period.TariffOperator,
                tariff.GetTotalCosts(),
                tariff.CreatedAt
            );
        }
        catch (ApplicationCaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationCaseException($"Error al obtener la tarifa: {ex.Message}", ex);
        }
    }
}