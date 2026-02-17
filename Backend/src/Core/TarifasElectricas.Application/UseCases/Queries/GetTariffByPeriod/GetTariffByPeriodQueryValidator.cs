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
            .Must(year => !year.HasValue || year.Value >= 1900)
            .WithMessage("El año debe ser mayor o igual a 1900")
            .Must(year => !year.HasValue || year.Value <= DateTime.UtcNow.Year + 1)
            .WithMessage($"El año no puede ser mayor a {DateTime.UtcNow.Year + 1}");

        RuleFor(x => x.Period)
            .MaximumLength(100)
            .WithMessage("El período no puede exceder 100 caracteres");

        RuleFor(x => x.TariffOperator)
            .MaximumLength(300)
            .WithMessage("El operador no puede exceder 300 caracteres");

        RuleFor(x => x.Level)
            .MaximumLength(120)
            .WithMessage("El nivel no puede exceder 120 caracteres");
    }
}
