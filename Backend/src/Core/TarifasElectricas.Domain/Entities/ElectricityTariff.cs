using System;
namespace TarifasElectricas.Domain.Entities;

using TarifasElectricas.Domain.Entities.Root;
using TarifasElectricas.Domain.Exceptions;
using TarifasElectricas.Domain.ValueObjects;

/// <summary>
/// Entidad que representa una tarifa de energía eléctrica del mercado regulado.
/// 
/// Una tarifa es la combinación de:
/// - Un período temporal (año, período específico de Gov.co)
/// - Un nivel/categoría de cliente (Nivel 1, NIVEL II, etc)
/// - Una empresa operadora (ENEL, CELSIA, etc)
/// - Un conjunto de componentes de costo (9 componentes de Gov.co)
/// 
/// Fuente: Dataset "Superservicios Tarifas Publicadas Energía" de Gov.co
/// Actualización: Mensual (aproximadamente)
/// 
/// La tarifa se utiliza para:
/// 1. Persistir datos crudos de Gov.co en la BD
/// 2. Simular facturas de consumo eléctrico
/// 3. Proporcionar información educativa sobre componentes de costo
/// </summary>
public class ElectricityTariff : AuditableEntity
{
    /// <summary>
    /// Período temporal y contextual de la tarifa
    /// (Year, Period, Level, Operator)
    /// </summary>
    public TariffPeriod Period { get; private set; } = null!;

    /// <summary>
    /// Componentes de costo (9 valores numéricos de Gov.co)
    /// </summary>
    public TariffCosts Costs { get; private set; } = null!;

    /// <summary>
    /// Referencia a la empresa/operador distribuidor
    /// 
    /// Foreign Key que apunta a Company.Id
    /// Necesaria para:
    /// - Integridad referencial en BD
    /// - Consultas por operador
    /// - Simulación de factura (para mostrar empresa)
    /// </summary>
    public Guid CompanyId { get; private set; }

    /// <summary>
    /// Constructor privado requerido por EF Core
    /// </summary>
    private ElectricityTariff() { }

    /// <summary>
    /// Constructor público para crear una nueva tarifa.
    /// 
    /// Parámetros:
    ///   period: Período, nivel, operador (TariffPeriod VO)
    ///   costs: 9 componentes de costo (TariffCosts VO)
    ///   companyId: ID de la empresa/operador (FK a Company)
    /// 
    /// Validaciones:
    ///   - period no puede ser null
    ///   - costs no puede ser null
    ///   - companyId no puede ser Guid.Empty
    /// </summary>
    public ElectricityTariff(TariffPeriod period, TariffCosts costs, Guid companyId)
    {
        if (period == null)
            throw new DomainRuleException("El período es requerido");

        if (costs == null)
            throw new DomainRuleException("Los costos son requeridos");

        if (companyId == Guid.Empty)
            throw new DomainRuleException("El ID de la empresa es requerido");

        Id = Guid.CreateVersion7();
        Period = period;
        Costs = costs;
        CompanyId = companyId;
        CreatedAt = DateTime.UtcNow;
        DateUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Actualiza los componentes de costo de la tarifa
    /// (cuando se reciben datos actualizados de Gov.co)
    /// 
    /// Parámetro:
    ///   newCosts: Nuevos componentes de costo (TariffCosts VO)
    /// </summary>
    public void UpdateCosts(TariffCosts newCosts)
    {
        if (newCosts == null)
            throw new DomainRuleException("Los costos son requeridos");

        Costs = newCosts;
        DateUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Simula una factura para un consumo en kWh.
    /// Devuelve el desglose por componentes y el total.
    /// </summary>
    public InvoiceSimulation SimulateInvoice(decimal kwhConsumption)
    {
        if (kwhConsumption <= 0)
            throw new DomainRuleException("El consumo en kWh debe ser mayor a 0");

        var consumptionCost = kwhConsumption * (Costs.TotalCu ?? 0);
        var transportCost = kwhConsumption *
            ((Costs.ChargeTransportStnTm ?? 0) + (Costs.ChargeTransportSdlDm ?? 0));
        var marketingCost = kwhConsumption * (Costs.MarketingMargin ?? 0);
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

        return new InvoiceSimulation(
            kwhConsumption,
            consumptionCost,
            transportCost,
            marketingCost,
            totalCost,
            components);
    }

    /// <summary>
    /// Calcula el costo total de la tarifa sumando todos los componentes
    /// 
    /// Utilidad: Para mostrar el total en factura simulada
    /// </summary>
    public decimal GetTotalCosts() => Costs.CalculateTotal();

    /// <summary>
    /// Retorna una representación legible de la tarifa para debugging
    /// Formato: "Año/Período - Nivel - Operador - CostoTotal"
    /// Ej: "2025/Enero - Nivel 1 (Propiedad OR) - ENEL Bogotá - 810.46"
    /// </summary>
    public override string ToString()
    {
        var totalCosts = GetTotalCosts();
        var totalCu = Costs.TotalCu ?? 0;
        return FormattableString.Invariant(
            $"{Period} - Costo Total: {totalCosts} - CU: {totalCu}");
    }
}
