using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using TarifasElectricas.Identity.Models;

namespace TarifasElectricas.Test.Infrastructure.Identity;

public class AppPoliciesTests
{
    [Fact]
    public void Configure_AddsPolicyForTariffQueries()
    {
        var options = new AuthorizationOptions();
        AppPolicies.Configure(options);

        var policy = options.GetPolicy(AppPolicies.CanQueryTariffs);
        Assert.NotNull(policy);
        Assert.Contains(policy!.Requirements, r => r is ClaimsAuthorizationRequirement);
    }

    [Fact]
    public void Configure_AddsPolicyForEtl()
    {
        var options = new AuthorizationOptions();
        AppPolicies.Configure(options);

        var policy = options.GetPolicy(AppPolicies.CanRunEtl);
        Assert.NotNull(policy);
        Assert.Contains(policy!.Requirements, r => r is ClaimsAuthorizationRequirement);
    }
}
