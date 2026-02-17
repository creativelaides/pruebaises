using System;
using TarifasElectricas.Domain.Entities.Root;

namespace TarifasElectricas.Application.Contracts.Repositories.Generic;

/// <summary>
/// Interfaz genérica para operaciones de persistencia
/// </summary>
public interface IRepository<T> where T : RootEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(Guid id);
}
