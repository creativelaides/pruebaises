using System;
using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Commands.CreateTariff;

/// <summary>
/// Validador para CreateTariffCommand usando FluentValidation
/// WolverineFx lo invoca automáticamente
/// </summary>
public class CreateTariffCommandValidator : AbstractValidator<CreateTariffCommand>
{
    public CreateTariffCommandValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(1900)
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
            .WithMessage("El año debe estar entre 1900 y el año actual + 1");

        RuleFor(x => x.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12)
            .WithMessage("El mes debe estar entre 1 y 12");

        RuleFor(x => x.Period)
            .NotEmpty()
            .WithMessage("El período de tarifa es requerido");

        RuleFor(x => x.Level)
            .NotEmpty()
            .WithMessage("El nivel de tensión es requerido");

        RuleFor(x => x.Operator)
            .NotEmpty()
            .WithMessage("El operador distribuidora es requerido");

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