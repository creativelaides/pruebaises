using TarifasElectricas.Application.Contracts.Repositories;
using TarifasElectricas.Application.Exceptions;

namespace TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;

public class SimulateInvoiceQueryHandler(
    IElectricityTariffRepository tariffs,
    ICompanyRepository companies)
{
    private readonly IElectricityTariffRepository _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));
    private readonly ICompanyRepository _companies = companies ?? throw new ArgumentNullException(nameof(companies));

    public async Task<SimulateInvoiceResponse> Handle(SimulateInvoiceQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await HandlerGuard.ExecuteAsync(async () =>
        {
            var tariff = await _tariffs.GetByIdAsync(query.TariffId);

            if (tariff == null)
                throw new ApplicationCaseException($"Tarifa con ID {query.TariffId} no encontrada");

            // ✅ NUEVO: Obtener Company para response
            var company = await _companies.GetByIdAsync(tariff.CompanyId);
            if (company == null)
                throw new ApplicationCaseException("Empresa no encontrada");

            var simulation = tariff.SimulateInvoice(query.KwhConsumption);

            return new SimulateInvoiceResponse(
                query.TariffId,
                company.Id,
                company.Code,
                simulation.KwhConsumption,
                simulation.ConsumptionCost,
                simulation.TransportCost,
                simulation.MarketingCost,
                simulation.TotalCost,
                simulation.Components);
        }, "Error al simular la factura");
    }
}
