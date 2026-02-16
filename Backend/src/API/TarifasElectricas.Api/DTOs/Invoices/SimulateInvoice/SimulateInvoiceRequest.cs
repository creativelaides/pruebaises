namespace TarifasElectricas.Api.DTOs.Invoices.SimulateInvoice;

public record SimulateInvoiceRequest(
    Guid TariffId,
    decimal KwhConsumption);
