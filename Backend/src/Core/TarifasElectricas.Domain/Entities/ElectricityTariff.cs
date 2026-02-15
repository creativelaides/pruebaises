using System;
using TarifasElectricas.Domain.Entities.EntityRoot;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Domain.Entities;

public class ElectricityTariff : Root
{
    public TariffPeriod Period { get; set; } = null!;
    public TariffCosts Costs { get; set; } = null!;
    public DateTime DateUpdated { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private ElectricityTariff() { }

    /// <summary>
    /// Constructor público para crear una nueva tarifa eléctrica
    /// </summary>
    public ElectricityTariff(TariffPeriod period, TariffCosts costs)
    {
        Id = Guid.CreateVersion7();
        Period = period;
        Costs = costs;
        CreatedAt = DateTime.UtcNow;
        DateUpdated = DateTime.UtcNow;
    }

    public void UpdateCosts(TariffCosts newCosts)
    {
        Costs = newCosts;
        DateUpdated = DateTime.UtcNow;
    }

    public decimal GetTotalCosts() => Costs.CalculateTotalComponents();
}