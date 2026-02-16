using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Enums;
using TarifasElectricas.Persistence.Repositories.Generic;

namespace TarifasElectricas.Persistence.Repositories;

public class EtlLogRepository : Repository<EtlLog>, IEtlLogRepository
{
    public EtlLogRepository(TariffDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<EtlLog>> GetByStateAsync(EtlState state) =>
        await Set.AsNoTracking()
            .Where(l => l.State == state)
            .OrderByDescending(l => l.ExecutionDate)
            .ToListAsync();

    public async Task<IEnumerable<EtlLog>> GetRecentLogsAsync(int days)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);
        return await Set.AsNoTracking()
            .Where(l => l.ExecutionDate >= fromDate)
            .OrderByDescending(l => l.ExecutionDate)
            .ToListAsync();
    }

    public Task<EtlLog?> GetLatestAsync() =>
        Set.AsNoTracking()
            .OrderByDescending(l => l.ExecutionDate)
            .FirstOrDefaultAsync();

    public async Task<decimal> GetSuccessRateAsync(int days)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);

        var total = await Set.AsNoTracking()
            .CountAsync(l => l.ExecutionDate >= fromDate);

        if (total == 0)
            return 0m;

        var success = await Set.AsNoTracking()
            .CountAsync(l => l.ExecutionDate >= fromDate && l.State == EtlState.Success);

        return (decimal)success / total;
    }
}
