using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TarifasElectricas.Application.Contracts.Identity;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Persistence;

namespace TarifasElectricas.Test.Infrastructure.Persistence;

public class AuditingTests
{
    [Fact]
    public async Task SaveChangesAsync_SetsCreatedAndUpdatedBy_WhenAdded()
    {
        var userService = Substitute.For<IAppUserService>();
        userService.GetUserId().Returns("user-1");

        await using var context = CreateContext(userService);
        var company = new Company("ENEL Bogotá - Cundinamarca");

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        Assert.Equal("user-1", company.CreatedBy);
        Assert.Equal("user-1", company.UpdatedBy);
        Assert.NotEqual(default, company.CreatedAt);
        Assert.NotEqual(default, company.DateUpdated);
    }

    [Fact]
    public async Task SaveChangesAsync_UpdatesUpdatedBy_WhenModified()
    {
        var userService = Substitute.For<IAppUserService>();
        userService.GetUserId().Returns("user-1");

        await using var context = CreateContext(userService);
        var company = new Company("ENEL Bogotá - Cundinamarca");

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        var originalUpdatedAt = company.DateUpdated;

        userService.GetUserId().Returns("user-2");
        company.UpdateCode("ENEL Bogotá - Cundinamarca 2");
        await context.SaveChangesAsync();

        Assert.Equal("user-2", company.UpdatedBy);
        Assert.True(company.DateUpdated >= originalUpdatedAt);
        Assert.Equal("user-1", company.CreatedBy);
    }

    [Fact]
    public async Task SaveChangesAsync_AllowsNullUser()
    {
        await using var context = CreateContext(null);
        var company = new Company("CELSIA Colombia - Valle del Cauca");

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        Assert.Null(company.CreatedBy);
        Assert.Null(company.UpdatedBy);
    }

    private static TariffDbContext CreateContext(IAppUserService? userService)
    {
        var options = new DbContextOptionsBuilder<TariffDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TariffDbContext(options, userService);
    }
}
