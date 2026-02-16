using TarifasElectricas.Application.Contracts.Repositories;

namespace TarifasElectricas.Application.Contracts.Persistence;

/// <summary>
/// Coordinador de repositorios y transacciones
/// Gestiona la persistencia de todas las entidades del dominio
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Repositorio para ElectricityTariff
    /// Métodos: GetByPeriodAsync, GetByYearAsync, GetLatestAsync
    /// </summary>
    IElectricityTariffRepository ElectricityTariffs { get; }

    /// <summary>
    /// Repositorio para Company
    /// Métodos: GetByCodeAsync, GetActiveCompaniesAsync, CodeExistsAsync
    /// </summary>
    ICompanyRepository Companies { get; }

    /// <summary>
    /// Repositorio para EtlLog
    /// Métodos: GetByStateAsync, GetRecentLogsAsync
    /// </summary>
    IEtlLogRepository EtlLogs { get; }

    /// <summary>
    /// Guarda todos los cambios pendientes en la BD
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Inicia una transacción
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Confirma la transacción actual
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Revierte la transacción actual
    /// </summary>
    Task RollbackAsync();
}