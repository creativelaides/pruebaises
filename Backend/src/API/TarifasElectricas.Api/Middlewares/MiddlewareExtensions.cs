namespace TarifasElectricas.Api.Middlewares;

/// <summary>
/// Extensiones para registrar middlewares propios de la API.
/// </summary>
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app) =>
        app.UseMiddleware<ApiExceptionHandlingMiddleware>();
}
