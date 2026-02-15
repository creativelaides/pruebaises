using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Mapster;
using Wolverine;

namespace TarifasElectricas.Application;

public static class DependencyInjectionApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Add Mapster
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        // ✅ WolverineFx: Descubre automáticamente TODOS los handlers
        // Busca métodos Handle/HandleAsync en el assembly
        // NO necesitas registrar handlers manualmente
        services.AddWolverine(options =>
        {
            // Descubre handlers en este assembly
            options.Discovery.IncludeAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
