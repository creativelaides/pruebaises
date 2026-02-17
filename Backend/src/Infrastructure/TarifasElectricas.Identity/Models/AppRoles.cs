namespace TarifasElectricas.Identity.Models;

/// <summary>
/// Roles de la aplicación y validaciones de pertenencia a grupos.
/// </summary>
public static class AppRoles
{
    public const string Admin = nameof(Admin);
    public const string Client = nameof(Client);

    public static readonly IReadOnlyList<string> All = new[] { Admin, Client };

    /// <summary>
    /// Valida el grupo lógico al que pertenece un rol.
    /// Ejemplo conceptual: Comercial -> Ventas, Developer -> IT.
    /// </summary>
    public static RoleGroupValidationResult ValidateRoleGroup(string role)
    {
        if (string.Equals(role, Admin, StringComparison.OrdinalIgnoreCase))
            return new RoleGroupValidationResult(true, Admin, "Operations", "El rol Admin pertenece al grupo Operations.");

        if (string.Equals(role, Client, StringComparison.OrdinalIgnoreCase))
            return new RoleGroupValidationResult(true, Client, "Customers", "El rol Client pertenece al grupo Customers.");

        return new RoleGroupValidationResult(false, role, string.Empty, "Rol no reconocido.");
    }
}

public sealed record RoleGroupValidationResult
(
    bool IsValid,
    string Role,
    string Group,
    string Message
);
