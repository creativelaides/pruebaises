using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Persistence.Repositories.Generic;

public class ElectricityTariffRepository : Repository<ElectricityTariff>, IElectricityTariffRepository
{
    public ElectricityTariffRepository(TariffDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ElectricityTariff>> GetByPeriodAsync(int year, string period)
    {
        var all = await Set.AsNoTracking().ToListAsync();
        return all.Where(t =>
            t.Period.Year == year &&
            string.Equals(t.Period.Period, period, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<ElectricityTariff>> GetByFiltersAsync(
        int? year,
        string? period,
        string? tariffOperator,
        string? level)
    {
        var all = await Set.AsNoTracking().ToListAsync();
        var query = all.AsEnumerable();

        if (year.HasValue)
            query = query.Where(t => t.Period.Year == year.Value);

        if (!string.IsNullOrWhiteSpace(period))
            query = query.Where(t => string.Equals(
                t.Period.Period,
                period.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(tariffOperator))
            query = query.Where(t => string.Equals(
                t.Period.TariffOperator,
                tariffOperator.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(level))
            query = query.Where(t => string.Equals(
                t.Period.Level,
                level.Trim(),
                StringComparison.OrdinalIgnoreCase));

        return query.ToList();
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
