namespace TarifasElectricas.Application.UseCases.Queries.GetTariffById;

/// <summary>
/// Query para obtener una tarifa eléctrica por su ID.
/// Record - Inmutable.
/// </summary>
public record GetTariffByIdQuery(Guid Id);
