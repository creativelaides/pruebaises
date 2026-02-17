using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TarifasElectricas.Application.Contracts.Identity;

namespace TarifasElectricas.Identity.Services;

/// <summary>
/// Servicio para obtener el usuario autenticado desde HttpContext.
/// </summary>
public sealed class AppUserService : IAppUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public string? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
