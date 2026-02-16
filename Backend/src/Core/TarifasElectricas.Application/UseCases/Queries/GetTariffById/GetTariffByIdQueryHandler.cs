using System;
using System.Threading.Tasks;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffById;

/// <summary>
/// Handler para GetTariffByIdQuery.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class GetTariffByIdQueryHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<GetTariffByIdResponse> Handle(GetTariffByIdQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var tariff = await _unitOfWork.ElectricityTariffs.GetByIdAsync(query.Id);

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