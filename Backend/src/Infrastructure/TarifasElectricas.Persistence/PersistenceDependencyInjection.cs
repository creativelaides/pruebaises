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
    /// <summary>
    /// Registra repositorios y unidad de trabajo.
    /// El DbContext (TariffDbContext) es registrado externamente por el host
    /// (Program.cs vía AddNpgsqlDbContext o AddDbContext según el entorno).
    /// </summary>
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IElectricityTariffRepository, ElectricityTariffRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEtlLogRepository, EtlLogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWorkEFCore>();

        return services;
    }
}
