using System;
using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Queries.GetTariffById;

/// <summary>
/// Validador para GetTariffByIdQuery.
/// WolverineFx lo invoca automáticamente.
/// </summary>
public class GetTariffByIdQueryValidator : AbstractValidator<GetTariffByIdQuery>
{
    public GetTariffByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El ID de la tarifa es requerido");
    }
}