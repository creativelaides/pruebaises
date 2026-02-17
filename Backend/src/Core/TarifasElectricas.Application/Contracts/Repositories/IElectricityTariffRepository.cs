using System;
using TarifasElectricas.Application.Contracts.Repositories.Generic;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Application.Contracts.Repositories;

/// <summary>
/// Interfaz específica para operaciones de persistencia de ElectricityTariff
/// 
/// Responsabilidades:
/// - GetByPeriodAsync: Obtener tarifas por año y período específico de Gov.co
/// - GetByYearAsync: Obtener todas las tarifas de un año
/// - GetLatestAsync: Obtener la tarifa más reciente
/// </summary>
public interface IElectricityTariffRepository : IRepository<ElectricityTariff>
{
    /// <summary>
    /// Obtiene tarifas por año y período específico.
    /// 
    /// Parámetros:
    ///   year: Año de la tarifa (ej: 2025)
    ///   period: Período específico de Gov.co (ej: "Enero", "Enero-Marzo")
    /// 
    /// Retorna:
    ///   IEnumerable<ElectricityTariff> (puede estar vacío)
    /// 
    /// Nota: El período de Gov.co ya incluye información temporal completa,
    /// no es necesario mes separado.
    /// </summary>
    Task<IEnumerable<ElectricityTariff>> GetByPeriodAsync(int year, string period);

    /// <summary>
    /// Obtiene todas las tarifas de un año específico.
    /// 
    /// Parámetro:
    ///   year: Año a consultar (ej: 2025)
    /// 
    /// Retorna:
    ///   IEnumerable<ElectricityTariff> (puede estar vacío)
    /// </summary>
    Task<IEnumerable<ElectricityTariff>> GetByYearAsync(int year);

    /// <summary>
    /// Obtiene la tarifa más reciente en la BD.
    /// Útil para: obtener la tarifa vigente, verificar última actualización
    /// 
    /// Retorna:
    ///   ElectricityTariff más reciente (por CreatedAt/DateUpdated), null si no hay tarifas
    /// </summary>
    Task<ElectricityTariff?> GetLatestAsync();
}
