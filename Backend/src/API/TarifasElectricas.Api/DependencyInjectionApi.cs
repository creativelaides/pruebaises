using Microsoft.Extensions.DependencyInjection;

namespace TarifasElectricas.Api
{
    public static class DependencyInjectionApi
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
    }
}
