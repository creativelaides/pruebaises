using Mapster;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Application.UseCases.Commands.CreateTariff;
using TarifasElectricas.Application.UseCases.Commands.UpdateTariff;
using TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;
using TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;
using TarifasElectricas.Application.UseCases.Queries.GetTariffById;
using TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;

namespace TarifasElectricas.Application.Mapping;

public static class MapsterConfig
{
    public static void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ElectricityTariff, GetTariffByIdResponse>()
            .MapWith(src => new GetTariffByIdResponse(
                src.Id,
                src.Period.Year,
                src.Period.Period,
                src.Period.Level,
                src.Period.TariffOperator,
                src.CompanyId,
                src.GetTotalCosts(),
                src.CreatedAt));

        config.NewConfig<ElectricityTariff, GetTariffByPeriodResponse>()
            .MapWith(src => new GetTariffByPeriodResponse(
                src.Id,
                src.Period.Year,
                src.Period.Period,
                src.Period.Level,
                src.Period.TariffOperator,
                src.CompanyId,
                src.GetTotalCosts(),
                src.CreatedAt));

        config.NewConfig<ElectricityTariff, GetLatestTariffResponse>()
            .MapWith(src => new GetLatestTariffResponse(
                src.Id,
                src.Period.Year,
                src.Period.Period,
                src.Period.Level,
                src.Period.TariffOperator,
                src.CompanyId,
                src.GetTotalCosts(),
                src.CreatedAt));

        config.NewConfig<ElectricityTariff, GetAllTariffsResponse.TariffItem>()
            .MapWith(src => new GetAllTariffsResponse.TariffItem(
                src.Id,
                src.Period.Year,
                src.Period.Period,
                src.Period.Level,
                src.Period.TariffOperator,
                src.CompanyId,
                src.GetTotalCosts(),
                src.CreatedAt));

        config.NewConfig<ElectricityTariff, CreateTariffResponse>()
            .MapWith(src => new CreateTariffResponse(
                src.Id,
                src.Period.Year,
                src.Period.Period,
                src.Period.Level,
                src.Period.TariffOperator,
                src.CompanyId,
                src.GetTotalCosts(),
                src.CreatedAt));

        config.NewConfig<ElectricityTariff, UpdateTariffResponse>()
            .MapWith(src => new UpdateTariffResponse(
                src.Id,
                src.Period.Year,
                src.Period.Period,
                src.Period.Level,
                src.Period.TariffOperator,
                src.CompanyId,
                src.GetTotalCosts(),
                src.DateUpdated));
    }
}
