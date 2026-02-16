namespace TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;

/// <summary>
/// Query para obtener todas las tarifas eléctricas.
/// Record - Inmutable.
/// </summary>
public record GetAllTariffsQuery(
    int Page = 1,
    int PageSize = 50);
