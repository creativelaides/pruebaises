using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Validador para CreateTariffCommand
/// Ejecutado automáticamente por WolverineFx antes del handler
/// 
/// Validaciones:
/// - Año: rango válido
/// - Período: no vacío
/// - Nivel: no vacío
/// - Operador: no vacío
/// - CompanyId: no vacío (NUEVO)
/// - Costos: no negativos
/// </summary>
public class CreateTariffCommandValidator : AbstractValidator<CreateTariffCommand>
{
    private static readonly HashSet<string> AllowedMonths = new(StringComparer.OrdinalIgnoreCase)
    {
        "Enero",
        "Febrero",
        "Marzo",
        "Abril",
        "Mayo",
        "Junio",
        "Julio",
        "Agosto",
        "Septiembre",
        "Octubre",
        "Noviembre",
        "Diciembre"
    };

    public CreateTariffCommandValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(1900)
            .WithMessage("El año debe ser mayor o igual a 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
            .WithMessage($"El año no puede ser mayor a {DateTime.UtcNow.Year + 1}");

        RuleFor(x => x.Period)
            .NotEmpty()
            .WithMessage("El período es requerido")
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .WithMessage("El período no puede estar vacío")
            .Must(p => p != null && AllowedMonths.Contains(p.Trim()))
            .WithMessage("El período debe ser un mes válido (Enero-Diciembre)")
            .MaximumLength(100)
            .WithMessage("El período no puede exceder 100 caracteres");

        RuleFor(x => x.Level)
            .NotEmpty()
            .WithMessage("El nivel es requerido")
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .WithMessage("El nivel no puede estar vacío")
            .MaximumLength(100)
            .WithMessage("El nivel no puede exceder 100 caracteres");

        RuleFor(x => x.TariffOperator)
            .NotEmpty()
            .WithMessage("El operador es requerido")
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .WithMessage("El operador no puede estar vacío")
            .MaximumLength(300)
            .WithMessage("El operador no puede exceder 300 caracteres");

        // ✅ NUEVO: Validar CompanyId
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("El ID de la empresa es requerido");

        // Validaciones de costos
        RuleFor(x => x.TotalCu)
            .GreaterThanOrEqualTo(0)
            .WithMessage("TotalCu no puede ser negativo")
            .When(x => x.TotalCu.HasValue);

        RuleFor(x => x.PurchaseCostG)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PurchaseCostG no puede ser negativo")
            .When(x => x.PurchaseCostG.HasValue);

        RuleFor(x => x.ChargeTransportStnTm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ChargeTransportStnTm no puede ser negativo")
            .When(x => x.ChargeTransportStnTm.HasValue);

        RuleFor(x => x.ChargeTransportSdlDm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ChargeTransportSdlDm no puede ser negativo")
            .When(x => x.ChargeTransportSdlDm.HasValue);

        RuleFor(x => x.MarketingMargin)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MarketingMargin no puede ser negativo")
            .When(x => x.MarketingMargin.HasValue);

        RuleFor(x => x.CostLossesPr)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CostLossesPr no puede ser negativo")
            .When(x => x.CostLossesPr.HasValue);

        RuleFor(x => x.RestrictionsRm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("RestrictionsRm no puede ser negativo")
            .When(x => x.RestrictionsRm.HasValue);

        RuleFor(x => x.Cot)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Cot no puede ser negativo")
            .When(x => x.Cot.HasValue);

        RuleFor(x => x.CfmjGfact)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CfmjGfact no puede ser negativo")
            .When(x => x.CfmjGfact.HasValue);
    }
}
