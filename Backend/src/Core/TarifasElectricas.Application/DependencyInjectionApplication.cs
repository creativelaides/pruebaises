using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Mapster;

namespace TarifasElectricas.Application
{
    public static class DependencyInjectionApplication
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add MediatR - assuming you'll use the non-abstracted version for implementation
            // services.AddMediatR(Assembly.GetExecutingAssembly());
            
            // Add FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add Mapster
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
