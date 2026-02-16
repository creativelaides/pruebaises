using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Enums;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Persistence;

public class TariffDbContext : DbContext
{
    public TariffDbContext(DbContextOptions<TariffDbContext> options) : base(options)
    {
    }

    public DbSet<ElectricityTariff> ElectricityTariffs => Set<ElectricityTariff>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<EtlLog> EtlLogs => Set<EtlLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TariffDbContext).Assembly);
    }
}
