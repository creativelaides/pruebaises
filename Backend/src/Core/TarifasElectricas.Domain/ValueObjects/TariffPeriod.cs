using TarifasElectricas.Domain.Exceptions;
namespace TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Value Object que representa el período temporal de una tarifa de energía.
/// 
/// Encapsula los datos de identificación temporal y contextual:
/// - Cuándo es válida la tarifa (año, período específico)
/// - A qué nivel/categoría aplica (Nivel 1, NIVEL II, etc)
/// - Qué operador la ofrece (nombre exacto de Gov.co)
/// 
/// Viene directamente del dataset de Gov.co sin transformación.
/// </summary>
public class TariffPeriod
{
    /// <summary>
    /// Año de la tarifa (ej: 2025)
    /// Validación: Entre 1900 y año actual + 1
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// Período específico dentro del año (ej: "Enero", "Enero-Marzo")
    /// Tal como viene del dataset de Gov.co
    /// Validación: No vacío, máximo 100 caracteres
    /// </summary>
    public string Period { get; init; } = null!;

    /// <summary>
    /// Nivel o categoría del cliente (ej: "Nivel 1 (Propiedad OR)", "NIVEL II")
    /// Tal como viene del dataset de Gov.co
    /// Validación: No vacío, máximo 100 caracteres
    /// </summary>
    public string Level { get; init; } = null!;

    /// <summary>
    /// Operador/empresa distribuidora (ej: "ENEL Bogotá - Cundinamarca")
    /// NOTA: Este valor viene de Gov.co y es duplicado de Company.Code
    /// Se mantiene aquí para preservar los datos tal como vienen del origen
    /// Validación: No vacío, máximo 300 caracteres
    /// </summary>
    public string TariffOperator { get; init; } = null!;

    /// <summary>
    /// Constructor principal con validaciones
    /// </summary>
    public TariffPeriod(int year, string period, string level, string tariffOperator)
    {
        ValidatePeriod(year, period, level, tariffOperator);

        Year = year;
        Period = period;
        Level = level;
        TariffOperator = tariffOperator;
    }

    /// <summary>
    /// Validaciones de las propiedades del período
    /// 
    /// Reglas:
    ///   - Year: Entre 1900 y año actual + 1 (para tarifas futuras)
    ///   - Period: No vacío (ej: "Enero", "Enero-Marzo", etc)
    ///   - Level: No vacío (ej: "Nivel 1 (Propiedad OR)", "NIVEL II", etc)
    ///   - TariffOperator: No vacío (operador_de_red de Gov.co)
    /// </summary>
    private static void ValidatePeriod(int year, string period, string level, string tariffOperator)
    {
        if (year < 1900 || year > DateTime.UtcNow.Year + 1)
            throw new DomainRuleException(
                $"El año debe estar entre 1900 y {DateTime.UtcNow.Year + 1}");

        if (string.IsNullOrWhiteSpace(period))
            throw new DomainRuleException("El período es requerido");

        if (period.Length > 100)
            throw new DomainRuleException("El período no puede exceder 100 caracteres");

        if (string.IsNullOrWhiteSpace(level))
            throw new DomainRuleException("El nivel es requerido");

        if (level.Length > 100)
            throw new DomainRuleException("El nivel no puede exceder 100 caracteres");

        if (string.IsNullOrWhiteSpace(tariffOperator))
            throw new DomainRuleException("El operador es requerido");

        if (tariffOperator.Length > 300)
            throw new DomainRuleException("El operador no puede exceder 300 caracteres");
    }

    /// <summary>
    /// Retorna una representación legible del período
    /// Ej: "2025 - Enero - Nivel 1 (Propiedad OR) - ENEL Bogotá - Cundinamarca"
    /// </summary>
    public override string ToString() =>
        $"{Year} - {Period} - {Level} - {TariffOperator}";

    /// <summary>
    /// Igualdad por valor (compara todas las propiedades)
    /// </summary>
    public override bool Equals(object? obj) =>
        obj is TariffPeriod other &&
        Year == other.Year &&
        Period == other.Period &&
        Level == other.Level &&
        TariffOperator == other.TariffOperator;

    /// <summary>
    /// Hash code basado en todas las propiedades
    /// </summary>
    public override int GetHashCode() =>
        HashCode.Combine(Year, Period, Level, TariffOperator);
}