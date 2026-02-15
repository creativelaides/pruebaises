using System;
using TarifasElectricas.Application.Contracts.Repositories;

namespace TarifasElectricas.Application.Contracts.Persistence;

/// <summary>
/// Unit of Work para gestionar transacciones y coordinación de repositorios
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IElectricityTariffRepository ElectricityTariffs { get; }
    IEducationComponentRepository EducationComponents { get; }
    IEtlLogRepository EtlLogs { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
