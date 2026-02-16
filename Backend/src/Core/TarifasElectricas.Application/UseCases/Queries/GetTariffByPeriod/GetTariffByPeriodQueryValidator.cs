using System;
using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Validador para GetTariffByPeriodQuery
/// FluentValidation + WolverineFx ejecutan automáticamente
/// 
/// Validaciones:
/// - Year: rango válido (1900 - año actual + 1)
/// - Period: no vacío (es el período de Gov.co: "Enero", "Enero-Marzo", etc)
/// </summary>
public class GetTariffByPeriodQueryValidator : AbstractValidator<GetTariffByPeriodQuery>
{
    public GetTariffByPeriodQueryValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(1900)
            .WithMessage("El año debe ser mayor o igual a 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
            .WithMessage($"El año no puede ser mayor a {DateTime.UtcNow.Year + 1}");

        // ✅ CORREGIDO: Period es string, no Month (int)
        RuleFor(x => x.Period)
            .NotEmpty()
            .WithMessage("El período es requerido")
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .WithMessage("El período no puede estar vacío")
            .MaximumLength(100)
            .WithMessage("El período no puede exceder 100 caracteres");
    }
}
