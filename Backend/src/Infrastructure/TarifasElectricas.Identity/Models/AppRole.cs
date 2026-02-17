using Microsoft.AspNetCore.Identity;

namespace TarifasElectricas.Identity.Models;

/// <summary>
/// Rol de la aplicación con metadata adicional.
/// </summary>
public class AppRole : IdentityRole
{
    public string? Description { get; set; }
    public int Level { get; set; }
}
