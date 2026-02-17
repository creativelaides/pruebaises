using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Identity.Models;

namespace TarifasElectricas.Identity;

/// <summary>
/// DbContext de Identity para usuarios y roles.
/// </summary>
public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }
}
