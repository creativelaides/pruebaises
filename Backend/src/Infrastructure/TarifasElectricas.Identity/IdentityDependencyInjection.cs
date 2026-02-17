using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarifasElectricas.Application.Contracts.Identity;
using TarifasElectricas.Identity.Models;
using TarifasElectricas.Identity.Services;

namespace TarifasElectricas.Identity;

/// <summary>
/// Registro de dependencias para Identity.
/// </summary>
public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' no configurada.");

        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddAuthentication(IdentityConstants.BearerScheme)
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorization(options =>
        {
            AppPolicies.Configure(options);
        });

        services.AddIdentityCore<AppUser>()
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddApiEndpoints();

        services.AddHttpContextAccessor();
        services.AddScoped<IAppUserService, AppUserService>();

        return services;
    }
}
