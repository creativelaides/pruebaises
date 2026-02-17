using System;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Query para obtener tarifas por filtros opcionales
/// 
/// Parámetros:
/// - Year: Año de la tarifa (ej: 2025)
/// - Period: Período de Gov.co (ej: "Enero", "Enero-Marzo")
/// - TariffOperator: Operador de red (ej: "ENEL Bogotá - Cundinamarca")
/// - Level: Nivel de tensión/servicio (ej: "NIVEL II")
/// </summary>
public record GetTariffByPeriodQuery(
    int? Year,
    string? Period,
    string? TariffOperator,
    string? Level);
