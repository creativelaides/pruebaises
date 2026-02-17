using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Mapster;
using TarifasElectricas.Application.Mapping;
using TarifasElectricas.Application.UseCases.Commands.CreateTariff;
using TarifasElectricas.Application.UseCases.Commands.DeleteTariff;
using TarifasElectricas.Application.UseCases.Commands.UpdateTariff;
using TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;
using TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;
using TarifasElectricas.Application.UseCases.Queries.GetTariffById;
using TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;
using TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;
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
        MapsterConfig.Register(typeAdapterConfig);
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        // ✅ WolverineFx: Descubre automáticamente TODOS los handlers
        // Busca métodos Handle/HandleAsync en el assembly
        // NO necesitas registrar handlers manualmente
        services.AddWolverine(options =>
        {
            // Descubre handlers en este assembly
            options.Discovery.IncludeAssembly(Assembly.GetExecutingAssembly());
        });

        // Handlers usados directamente por los controllers API.
        // Wolverine los descubre para mensajería, pero no los registra como servicios
        // concretos para inyección directa.
        services.AddScoped<CreateTariffCommandHandler>();
        services.AddScoped<UpdateTariffCommandHandler>();
        services.AddScoped<DeleteTariffCommandHandler>();
        services.AddScoped<GetTariffByIdQueryHandler>();
        services.AddScoped<GetTariffByPeriodQueryHandler>();
        services.AddScoped<GetLatestTariffQueryHandler>();
        services.AddScoped<GetAllTariffsQueryHandler>();
        services.AddScoped<SimulateInvoiceQueryHandler>();

        return services;
    }
}
