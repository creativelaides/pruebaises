using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarifasElectricas.Application.Contracts.Identity;
using TarifasElectricas.Identity.Models;
using TarifasElectricas.Identity.Services;

namespace TarifasElectricas.Identity;

/// <summary>
/// Registro de dependencias para Identity.
/// El DbContext (AppIdentityDbContext) es registrado externamente por el host
/// (Program.cs vía AddNpgsqlDbContext o AddDbContext según el entorno).
/// </summary>
public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
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
