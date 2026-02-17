using Microsoft.AspNetCore.Authorization;

namespace TarifasElectricas.Identity.Models;

/// <summary>
/// Políticas de autorización de la API.
/// </summary>
public static class AppPolicies
{
    public const string CanQueryTariffs = nameof(CanQueryTariffs);
    public const string CanRunEtl = nameof(CanRunEtl);

    public static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(CanQueryTariffs, policy =>
            policy.RequireClaim(AppClaims.PermissionType, AppClaims.TariffsRead));

        options.AddPolicy(CanRunEtl, policy =>
            policy.RequireClaim(AppClaims.PermissionType, AppClaims.EtlRun));
    }
}
