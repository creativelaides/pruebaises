namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Response del comando CreateTariffCommand
/// 
/// Retorna los datos de la tarifa creada:
/// - ID único
/// - Período (Year, Period, Level, Operator)
/// - ID de la empresa creadora
/// - Total de costos
/// - Timestamp de creación
/// </summary>
public record CreateTariffResponse(
    Guid Id,
    int Year,
    string? Period,
    string? Level,
    string? TariffOperator,
    Guid CompanyId,
    decimal TotalCosts,
    DateTime CreatedAt);