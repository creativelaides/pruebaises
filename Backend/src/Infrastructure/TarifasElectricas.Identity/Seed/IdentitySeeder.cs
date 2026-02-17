using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarifasElectricas.Identity.Models;

namespace TarifasElectricas.Identity.Seed;

/// <summary>
/// Seed inicial de roles, claims y usuarios base.
/// </summary>
public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var scoped = scope.ServiceProvider;

        var roleManager = scoped.GetRequiredService<RoleManager<AppRole>>();
        var userManager = scoped.GetRequiredService<UserManager<AppUser>>();

        await EnsureRoleAsync(roleManager, AppRoles.Admin);
        await EnsureRoleAsync(roleManager, AppRoles.Client);

        await EnsureRoleClaimsAsync(roleManager, AppRoles.Admin);
        await EnsureRoleClaimsAsync(roleManager, AppRoles.Client);

        var adminSection = configuration.GetSection("SeedUsers:Admin");
        await EnsureUserAsync(userManager, adminSection, AppRoles.Admin);

        var clientSection = configuration.GetSection("SeedUsers:Client");
        await EnsureUserAsync(userManager, clientSection, AppRoles.Client);
    }

    private static async Task EnsureRoleAsync(RoleManager<AppRole> roleManager, string role)
    {
        if (await roleManager.RoleExistsAsync(role))
            return;

        var appRole = new AppRole
        {
            Name = role,
            Description = role == AppRoles.Admin ? "Administrador del sistema." : "Cliente con permisos de consulta.",
            Level = role == AppRoles.Admin ? 100 : 10
        };

        var result = await roleManager.CreateAsync(appRole);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"No se pudo crear el rol '{role}': {string.Join("; ", result.Errors.Select(e => e.Description))}");
    }

    private static async Task EnsureRoleClaimsAsync(RoleManager<AppRole> roleManager, string role)
    {
        var identityRole = await roleManager.FindByNameAsync(role);
        if (identityRole == null)
            return;

        var existing = await roleManager.GetClaimsAsync(identityRole);
        foreach (var claimValue in AppClaims.ForRole(role))
        {
            if (existing.Any(c => c.Type == AppClaims.PermissionType && c.Value == claimValue))
                continue;

            await roleManager.AddClaimAsync(identityRole, new Claim(AppClaims.PermissionType, claimValue));
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<AppUser> userManager,
        IConfigurationSection section,
        string role)
    {
        var username = section["Username"];
        var email = section["Email"];
        var password = section["Password"];

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
            return;

        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = new AppUser
            {
                UserName = username,
                Email = email
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                throw new InvalidOperationException(
                    $"No se pudo crear el usuario '{username}': {string.Join("; ", createResult.Errors.Select(e => e.Description))}");
        }

        if (!await userManager.IsInRoleAsync(user, role))
            await userManager.AddToRoleAsync(user, role);
    }
}
