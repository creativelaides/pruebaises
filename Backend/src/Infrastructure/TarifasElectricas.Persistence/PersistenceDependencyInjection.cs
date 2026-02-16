using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Persistence.Repositories;
using TarifasElectricas.Persistence.Repositories.Generic;
using TarifasElectricas.Persistence.UnitsOfWork;

namespace TarifasElectricas.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' no configurada.");

        services.AddDbContext<TariffDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IElectricityTariffRepository, ElectricityTariffRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEtlLogRepository, EtlLogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWorkEFCore>();

        return services;
    }
}
