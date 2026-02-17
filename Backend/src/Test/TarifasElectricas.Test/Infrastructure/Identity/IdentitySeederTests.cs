using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarifasElectricas.Identity;
using TarifasElectricas.Identity.Models;
using TarifasElectricas.Identity.Seed;

namespace TarifasElectricas.Test.Infrastructure.Identity;

public class IdentitySeederTests
{
    [Fact]
    public async Task SeedAsync_CreatesRoles()
    {
        var provider = BuildProvider();
        var configuration = BuildConfiguration();

        await IdentitySeeder.SeedAsync(provider, configuration);

        using var scope = provider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        Assert.True(await roleManager.RoleExistsAsync(AppRoles.Admin));
        Assert.True(await roleManager.RoleExistsAsync(AppRoles.Client));
    }

    [Fact]
    public async Task SeedAsync_AssignsRoleClaims()
    {
        var provider = BuildProvider();
        var configuration = BuildConfiguration();

        await IdentitySeeder.SeedAsync(provider, configuration);

        using var scope = provider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        var adminRole = await roleManager.FindByNameAsync(AppRoles.Admin);
        var clientRole = await roleManager.FindByNameAsync(AppRoles.Client);

        var adminClaims = await roleManager.GetClaimsAsync(adminRole!);
        var clientClaims = await roleManager.GetClaimsAsync(clientRole!);

        Assert.Contains(adminClaims, c => c.Type == AppClaims.PermissionType && c.Value == AppClaims.TariffsRead);
        Assert.Contains(adminClaims, c => c.Type == AppClaims.PermissionType && c.Value == AppClaims.EtlRun);
        Assert.Contains(clientClaims, c => c.Type == AppClaims.PermissionType && c.Value == AppClaims.TariffsRead);
        Assert.DoesNotContain(clientClaims, c => c.Type == AppClaims.PermissionType && c.Value == AppClaims.EtlRun);
    }

    [Fact]
    public async Task SeedAsync_CreatesAdminUserWithRole()
    {
        var provider = BuildProvider();
        var configuration = BuildConfiguration();

        await IdentitySeeder.SeedAsync(provider, configuration);

        using var scope = provider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var admin = await userManager.FindByNameAsync("johndoe");

        Assert.NotNull(admin);
        Assert.True(await userManager.IsInRoleAsync(admin!, AppRoles.Admin));
    }

    [Fact]
    public async Task SeedAsync_CreatesClientUserWithRole()
    {
        var provider = BuildProvider();
        var configuration = BuildConfiguration();

        await IdentitySeeder.SeedAsync(provider, configuration);

        using var scope = provider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var client = await userManager.FindByNameAsync("client");

        Assert.NotNull(client);
        Assert.True(await userManager.IsInRoleAsync(client!, AppRoles.Client));
    }

    [Fact]
    public async Task SeedAsync_IsIdempotent_ForRoleClaims()
    {
        var provider = BuildProvider();
        var configuration = BuildConfiguration();

        await IdentitySeeder.SeedAsync(provider, configuration);
        await IdentitySeeder.SeedAsync(provider, configuration);

        using var scope = provider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        var adminRole = await roleManager.FindByNameAsync(AppRoles.Admin);
        var adminClaims = await roleManager.GetClaimsAsync(adminRole!);

        var permissionClaims = adminClaims
            .Where(c => c.Type == AppClaims.PermissionType)
            .Select(c => c.Value)
            .ToList();

        Assert.Equal(2, permissionClaims.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    private static ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddOptions();

        var dbRoot = new InMemoryDatabaseRoot();
        var dbName = Guid.NewGuid().ToString();

        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseInMemoryDatabase(dbName, dbRoot));

        services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        db.Database.EnsureCreated();

        return provider;
    }

    private static IConfiguration BuildConfiguration()
    {
        var data = new Dictionary<string, string?>
        {
            ["SeedUsers:Admin:Username"] = "johndoe",
            ["SeedUsers:Admin:Email"] = "johndoe@example.com",
            ["SeedUsers:Admin:Password"] = "HelloWorldHorse9876*",
            ["SeedUsers:Client:Username"] = "client",
            ["SeedUsers:Client:Email"] = "client@example.com",
            ["SeedUsers:Client:Password"] = "HelloHorseWorld1234+"
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(data)
            .Build();
    }
}
