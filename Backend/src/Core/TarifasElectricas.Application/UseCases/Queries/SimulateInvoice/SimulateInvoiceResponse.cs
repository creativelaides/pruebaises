using System;
using System.Collections.Generic;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;

/// <summary>
/// Response con el desglose de la factura simulada.
/// </summary>
// Application/UseCases/Queries/SimulateInvoice/SimulateInvoiceResponse.cs

public record SimulateInvoiceResponse(
    Guid TariffId,
    Guid CompanyId,
    string CompanyName,
    decimal KwhConsumption,
    decimal ConsumptionCost,
    decimal TransportCost,
    decimal MarketingCost,
    decimal TotalCost,
    List<InvoiceComponent> Components);