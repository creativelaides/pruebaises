using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Enums;
using TarifasElectricas.Domain.ValueObjects;
using TarifasElectricas.Application.Contracts.Identity;
using TarifasElectricas.Domain.Entities.Root;

namespace TarifasElectricas.Persistence;

public class TariffDbContext : DbContext
{
    private readonly IAppUserService? _appUserService;

    public TariffDbContext(
        DbContextOptions<TariffDbContext> options,
        IAppUserService? appUserService = null) : base(options)
    {
        _appUserService = appUserService;
    }

    public DbSet<ElectricityTariff> ElectricityTariffs => Set<ElectricityTariff>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<EtlLog> EtlLogs => Set<EtlLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TariffDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _appUserService?.GetUserId();
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.DateUpdated = now;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.UpdatedBy = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.DateUpdated = now;
                    entry.Entity.UpdatedBy = userId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
