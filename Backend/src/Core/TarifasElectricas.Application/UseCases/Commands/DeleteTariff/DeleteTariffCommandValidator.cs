using System;
using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Commands.DeleteTariff;

/// <summary>
/// Validador para DeleteTariffCommand.
/// WolverineFx lo invoca automáticamente.
/// </summary>
public class DeleteTariffCommandValidator : AbstractValidator<DeleteTariffCommand>
{
    public DeleteTariffCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El ID de la tarifa es requerido");
    }
}
