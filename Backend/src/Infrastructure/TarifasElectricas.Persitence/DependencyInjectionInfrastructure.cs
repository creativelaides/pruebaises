using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarifasElectricas.Domain.Interfaces;
using TarifasElectricas.Infrastructure.Data;
using TarifasElectricas.Infrastructure.Repositories;
using TarifasElectricas.Infrastructure.ExternalServices;

namespace TarifasElectricas.Infrastructure
{
    public static class DependencyInjectionInfrastructure
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext
            services.AddDbContext<TarifasDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            services.AddScoped<IElectricityTariffRepository, TarifaRepository>();

            // Register HttpClient for GovCoApiClient
            services.AddHttpClient<GovCoApiClient>();

            return services;
        }
    }
}
