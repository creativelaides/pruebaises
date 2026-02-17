using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Application.Contracts.Repositories.Generic;
using TarifasElectricas.Domain.Entities.Root;

namespace TarifasElectricas.Persistence.Repositories.Generic;

public class Repository<T> : IRepository<T> where T : RootEntity
{
    protected readonly TariffDbContext Context;
    protected readonly DbSet<T> Set;

    public Repository(TariffDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Set = Context.Set<T>();
    }

    public Task<T?> GetByIdAsync(Guid id) =>
        Set.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await Set.ToListAsync();

    public Task AddAsync(T entity) =>
        Set.AddAsync(entity).AsTask();

    public Task UpdateAsync(T entity)
    {
        Set.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        Set.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id) =>
        Set.AnyAsync(x => x.Id == id);
}
