using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Persistence.Repositories.Generic;

public class ElectricityTariffRepository : Repository<ElectricityTariff>, IElectricityTariffRepository
{
    public ElectricityTariffRepository(TariffDbContext context) : base(context)
    {
    }

    public async Task<ElectricityTariff?> GetByPeriodAsync(int year, string period)
    {
        var all = await Set.AsNoTracking().ToListAsync();
        return all.FirstOrDefault(t =>
            t.Period.Year == year &&
            string.Equals(t.Period.Period, period, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<ElectricityTariff>> GetByYearAsync(int year)
    {
        var all = await Set.AsNoTracking().ToListAsync();
        return all.Where(t => t.Period.Year == year);
    }

    public Task<ElectricityTariff?> GetLatestAsync() =>
        Set.AsNoTracking()
            .OrderByDescending(t => t.DateUpdated)
            .ThenByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();
}
