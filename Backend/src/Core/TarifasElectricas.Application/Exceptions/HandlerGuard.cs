using TarifasElectricas.Domain.Exceptions;

namespace TarifasElectricas.Application.Exceptions;

public static class HandlerGuard
{
    public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action, string errorMessage)
    {
        try
        {
            return await action();
        }
        catch (DomainRuleException ex)
        {
            throw new ApplicationCaseException(
                $"Error de validación en el dominio: {ex.Message}", ex);
        }
        catch (ApplicationCaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationCaseException($"{errorMessage}: {ex.Message}", ex);
        }
    }
}
