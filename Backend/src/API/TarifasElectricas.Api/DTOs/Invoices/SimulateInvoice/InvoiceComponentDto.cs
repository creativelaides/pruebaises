namespace TarifasElectricas.Api.DTOs.Invoices.SimulateInvoice;

public record InvoiceComponentDto(
    string Name,
    decimal Value,
    string Explanation);
