using System;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;

/// <summary>
/// Handler para GetAllTariffsQuery.
/// WolverineFx lo descubre automáticamente.
/// </summary>
public class GetAllTariffsQueryHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<GetAllTariffsResponse> Handle(GetAllTariffsQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var tariffs = await _unitOfWork.ElectricityTariffs.GetAllAsync();

            var tariffItems = tariffs
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
        }
        catch (Exception ex)
        {
            throw new ApplicationCaseException($"Error al obtener las tarifas: {ex.Message}", ex);
        }
    }
}

