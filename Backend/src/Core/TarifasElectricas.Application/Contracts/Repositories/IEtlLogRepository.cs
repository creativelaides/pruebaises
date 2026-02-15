using System;
using TarifasElectricas.Application.Contracts.Repositories.Generic;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Enums;

namespace TarifasElectricas.Application.Contracts.Repositories;

/// <summary>
/// Interfaz específica para operaciones de persistencia de EtlLog
/// </summary>
public interface IEtlLogRepository : IRepository<EtlLog>
{
    Task<IEnumerable<EtlLog>> GetByStateAsync(EtlState state);
    Task<IEnumerable<EtlLog>> GetRecentLogsAsync(int days);
    Task<EtlLog?> GetLatestAsync();
    Task<decimal> GetSuccessRateAsync(int days);
}
