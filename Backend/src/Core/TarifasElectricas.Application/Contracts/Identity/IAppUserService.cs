namespace TarifasElectricas.Application.Contracts.Identity;

/// <summary>
/// Provee información del usuario autenticado actual.
/// </summary>
public interface IAppUserService
{
    /// <summary>
    /// Retorna el ID del usuario actual, o null si no existe.
    /// </summary>
    string? GetUserId();
}
