using System;
using System.Threading.Tasks;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;

/// <summary>
/// Handler para GetLatestTariffQuery.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class GetLatestTariffQueryHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<GetLatestTariffResponse> Handle(GetLatestTariffQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var tariff = await _unitOfWork.ElectricityTariffs.GetLatestAsync();

            if (tariff == null)
                throw new ApplicationCaseException("No hay tarifas disponibles");

            return new GetLatestTariffResponse(
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
            throw new ApplicationCaseException($"Error al obtener la tarifa más reciente: {ex.Message}", ex);
        }
    }
}