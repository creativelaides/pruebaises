using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Persistence.Repositories.Generic;

namespace TarifasElectricas.Persistence.Repositories;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(TariffDbContext context) : base(context)
    {
    }

    public Task<Company?> GetByCodeAsync(string code) =>
        Set.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code);

    public Task<bool> CodeExistsAsync(string code) =>
        Set.AsNoTracking().AnyAsync(c => c.Code == code);

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync() =>
        await Set.AsNoTracking().ToListAsync();
}
