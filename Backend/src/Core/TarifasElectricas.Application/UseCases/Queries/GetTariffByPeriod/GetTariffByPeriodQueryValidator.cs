using System;
using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

/// <summary>
/// Validador para GetTariffByPeriodQuery.
/// WolverineFx lo invoca automáticamente.
/// </summary>
public class GetTariffByPeriodQueryValidator : AbstractValidator<GetTariffByPeriodQuery>
{
    public GetTariffByPeriodQueryValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(1900)
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
            .WithMessage("El año debe estar entre 1900 y el año actual + 1");

        RuleFor(x => x.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12)
            .WithMessage("El mes debe estar entre 1 y 12");
    }
}
