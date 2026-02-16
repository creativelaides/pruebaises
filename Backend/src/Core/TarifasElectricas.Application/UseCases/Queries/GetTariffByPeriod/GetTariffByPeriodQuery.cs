using System;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Query para obtener una tarifa por período específico
/// 
/// Parámetros:
/// - Year: Año de la tarifa (ej: 2025)
/// - Period: Período de Gov.co (ej: "Enero", "Enero-Marzo")
/// 
/// Nota: Month ya no es necesario porque Period de Gov.co incluye toda la información temporal
/// </summary>
public record GetTariffByPeriodQuery(int Year, string Period);