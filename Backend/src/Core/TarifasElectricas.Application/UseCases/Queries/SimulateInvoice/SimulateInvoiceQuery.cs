using System;

namespace TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;

/// <summary>
/// Query para simular una factura de energía eléctrica.
/// Entrada: ID de tarifa y consumo en kWh.
/// Salida: Desglose de costos por componente.
/// </summary>
public record SimulateInvoiceQuery(
    Guid TariffId,
    decimal KwhConsumption);