using System;
using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Commands.UpdateTariff;

/// <summary>
/// Validador para UpdateTariffCommand.
/// WolverineFx lo invoca automáticamente.
/// </summary>
public class UpdateTariffCommandValidator : AbstractValidator<UpdateTariffCommand>
{
    public UpdateTariffCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El ID de la tarifa es requerido");

        RuleFor(x => x.TotalCu)
            .GreaterThanOrEqualTo(0)
            .WithMessage("TotalCu no puede ser negativo");

        RuleFor(x => x.PurchaseCostG)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PurchaseCostG no puede ser negativo");

        RuleFor(x => x.ChargeTransportStnTm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ChargeTransportStnTm no puede ser negativo");

        RuleFor(x => x.ChargeTransportSdlDm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ChargeTransportSdlDm no puede ser negativo");

        RuleFor(x => x.MarketingMargin)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MarketingMargin no puede ser negativo");

        RuleFor(x => x.CostLossesPr)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CostLossesPr no puede ser negativo");

        RuleFor(x => x.RestrictionsRm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("RestrictionsRm no puede ser negativo");

        RuleFor(x => x.Cot)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Cot no puede ser negativo");

        RuleFor(x => x.CfmjGfact)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CfmjGfact no puede ser negativo");
    }
}
