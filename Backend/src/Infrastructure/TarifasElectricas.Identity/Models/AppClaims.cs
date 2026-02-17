namespace TarifasElectricas.Identity.Models;

/// <summary>
/// Claims de permisos para la API.
/// </summary>
public static class AppClaims
{
    public const string PermissionType = "permission";
    public const string TariffsRead = "tariffs.read";
    public const string EtlRun = "etl.run";

    public static IReadOnlyList<string> ForRole(string role) =>
        role switch
        {
            AppRoles.Admin => new[] { TariffsRead, EtlRun },
            AppRoles.Client => new[] { TariffsRead },
            _ => Array.Empty<string>()
        };
}
