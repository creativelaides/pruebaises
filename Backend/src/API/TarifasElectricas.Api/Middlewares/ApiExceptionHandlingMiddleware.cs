using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Api.Middlewares;

/// <summary>
/// Maneja excepciones no controladas y responde con ProblemDetails.
/// </summary>
public sealed class ApiExceptionHandlingMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ApplicationCaseException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError,
                "Ocurrió un error inesperado.", ex);
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        HttpStatusCode status,
        string message,
        Exception? exception = null)
    {
        if (context.Response.HasStarted)
            return;

        var problem = new ProblemDetails
        {
            Title = status == HttpStatusCode.InternalServerError ? "Error interno" : "Solicitud inválida",
            Status = (int)status,
            Detail = message,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        if (exception != null)
            problem.Extensions["exception"] = exception.GetType().Name;

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problem.Status ?? (int)status;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
    }
}
