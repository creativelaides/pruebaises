using System;
using TarifasElectricas.Application.Contracts.Repositories.Generic;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Application.Contracts.Repositories;

/// <summary>
/// Interfaz específica para operaciones de persistencia de ElectricityTariff
/// </summary>
public interface IElectricityTariffRepository : IRepository<ElectricityTariff>
{
    Task<ElectricityTariff?> GetByPeriodAsync(int year, int month);
    Task<IEnumerable<ElectricityTariff>> GetByYearAsync(int year);
    Task<ElectricityTariff?> GetLatestAsync();
}
