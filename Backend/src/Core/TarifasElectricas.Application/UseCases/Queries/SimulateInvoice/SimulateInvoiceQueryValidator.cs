using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;

/// <summary>
/// Validador para SimulateInvoiceQuery.
/// </summary>
public class SimulateInvoiceQueryValidator : AbstractValidator<SimulateInvoiceQuery>
{
    public SimulateInvoiceQueryValidator()
    {
        RuleFor(x => x.TariffId)
            .NotEmpty()
            .WithMessage("El ID de la tarifa es requerido");

        RuleFor(x => x.KwhConsumption)
            .GreaterThan(0)
            .WithMessage("El consumo en kWh debe ser mayor a 0");
    }
}
