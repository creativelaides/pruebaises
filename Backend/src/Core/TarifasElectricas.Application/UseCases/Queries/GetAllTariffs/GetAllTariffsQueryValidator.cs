using FluentValidation;

namespace TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;

/// <summary>
/// Validador para GetAllTariffsQuery.
/// </summary>
public class GetAllTariffsQueryValidator : AbstractValidator<GetAllTariffsQuery>
{
    public GetAllTariffsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page debe ser mayor o igual a 1");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 200)
            .WithMessage("PageSize debe estar entre 1 y 200");
    }
}
