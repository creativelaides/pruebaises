using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TarifasElectricas.Application.Contracts.Services;
using TarifasElectricas.Infrastructure.Options;
using TarifasElectricas.Infrastructure.Services;

namespace TarifasElectricas.Infrastructure;

/// <summary>
/// Registro de dependencias para la capa Infrastructure.
/// </summary>
public static class DependencyInjectionInfrastructure
{
    /// <summary>
    /// Registra servicios de infraestructura (HTTP client, ETL y opciones).
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SocrataOptions>(configuration.GetSection(SocrataOptions.SectionName));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

        services.AddHttpClient<SocrataClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<SocrataOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });

        services.AddScoped<IEtlService, SocrataEtlService>();
        services.AddScoped<IEmailService, SmtpEmailService>();

        return services;
    }
}
