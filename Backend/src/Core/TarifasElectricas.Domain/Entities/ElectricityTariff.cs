using System;
using TarifasElectricas.Domain.Entities.EntityRoot;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Domain.Entities
{
    
    public class ElectricityTariff : Root
    {
        public TariffPeriod Period { get; set; } = null!;
        public TariffCosts Costs { get; set; } = null!;
        public DateTime DateUpdated { get; set; }
        public DateTime CreatedAt { get; set; }

        public ElectricityTariff() { }

        public ElectricityTariff(TariffPeriod period, TariffCosts costs)
        {
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

        /// <summary>
        /// Calcula el valor total de la tarifa sumando sus componentes de costo.
        /// </summary>
        /// <returns>El costo total calculado.</returns>
        public decimal GetTotalCosts() => Costs.CalculateTotalComponents();
    }
}