namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Query para obtener una tarifa eléctrica por año y mes.
/// Record - Inmutable.
/// </summary>
public record GetTariffByPeriodQuery(int Year, int Month);