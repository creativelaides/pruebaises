using System.Collections.Generic;

namespace TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Resultado de una simulación de factura.
/// </summary>
public sealed class InvoiceSimulation
{
    public decimal KwhConsumption { get; }
    public decimal ConsumptionCost { get; }
    public decimal TransportCost { get; }
    public decimal MarketingCost { get; }
    public decimal TotalCost { get; }
    public IReadOnlyList<InvoiceComponent> Components { get; }

    public InvoiceSimulation(
        decimal kwhConsumption,
        decimal consumptionCost,
        decimal transportCost,
        decimal marketingCost,
        decimal totalCost,
        IReadOnlyList<InvoiceComponent> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        KwhConsumption = kwhConsumption;
        ConsumptionCost = consumptionCost;
        TransportCost = transportCost;
        MarketingCost = marketingCost;
        TotalCost = totalCost;
        Components = components;
    }
}
