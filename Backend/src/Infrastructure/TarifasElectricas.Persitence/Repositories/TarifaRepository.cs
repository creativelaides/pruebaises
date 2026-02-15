using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Interfaces;
using TarifasElectricas.Infrastructure.Data;

namespace TarifasElectricas.Infrastructure.Repositories
{
    public class TarifaRepository : IElectricityTariffRepository
    {
        private readonly TarifasDbContext _context;

        public TarifaRepository(TarifasDbContext context)
        {
            _context = context;
        }

        public async Task BulkInsertAsync(IEnumerable<TarifaElectrica> tarifas)
        {
            await _context.TarifasElectricas.AddRangeAsync(tarifas);
            await _context.SaveChangesAsync();
        }

        public async Task GuardarLogAsync(EtlLog log)
        {
            await _context.EtlLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
