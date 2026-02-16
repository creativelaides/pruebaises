using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;
using TarifasElectricas.Domain.ValueObjects;

public class SimulateInvoiceQueryHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<SimulateInvoiceResponse> Handle(SimulateInvoiceQuery query)
    {
        var tariff = await _unitOfWork.ElectricityTariffs.GetByIdAsync(query.TariffId);

        if (tariff == null)
            throw new ApplicationCaseException($"Tarifa con ID {query.TariffId} no encontrada");

        // ✅ NUEVO: Obtener Company para response
        var company = await _unitOfWork.Companies.GetByIdAsync(tariff.CompanyId);
        if (company == null)
            throw new ApplicationCaseException($"Empresa no encontrada");

        // Calcular componentes
        var consumptionCost = query.KwhConsumption * (tariff.Costs.TotalCu ?? 0);
        var transportCost = query.KwhConsumption *
            ((tariff.Costs.ChargeTransportStnTm ?? 0) + (tariff.Costs.ChargeTransportSdlDm ?? 0));
        var marketingCost = query.KwhConsumption * (tariff.Costs.MarketingMargin ?? 0);
        var totalCost = consumptionCost + transportCost + marketingCost;

        var components = new List<InvoiceComponent>();

        if (consumptionCost > 0)
        {
            components.Add(new InvoiceComponent(
                "Consumo de Energía",
                consumptionCost,
                "Lo que pagas por la electricidad que consumiste en tu hogar"));
        }

        if (transportCost > 0)
        {
            components.Add(new InvoiceComponent(
                "Transporte y Distribución",
                transportCost,
                "El costo de llevar la energía desde las plantas generadoras hasta tu casa"));
        }

        if (marketingCost > 0)
        {
            components.Add(new InvoiceComponent(
                "Comercialización",
                marketingCost,
                "El servicio de lectura del contador, facturación y atención al cliente"));
        }

        return new SimulateInvoiceResponse(
            query.TariffId,
            company.Id,
            company.Code,
            query.KwhConsumption,
            consumptionCost,
            transportCost,
            marketingCost,
            totalCost,
            components);
    }
}