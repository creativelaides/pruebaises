using Microsoft.EntityFrameworkCore.Storage;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Persistence.Repositories;
using TarifasElectricas.Persistence.Repositories.Generic;

namespace TarifasElectricas.Persistence.UnitsOfWork;

public class UnitOfWorkEFCore : IUnitOfWork
{
    private readonly TariffDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWorkEFCore(TariffDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        ElectricityTariffs = new ElectricityTariffRepository(_context);
        Companies = new CompanyRepository(_context);
        EtlLogs = new EtlLogRepository(_context);
    }

    public IElectricityTariffRepository ElectricityTariffs { get; }
    public ICompanyRepository Companies { get; }
    public IEtlLogRepository EtlLogs { get; }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
