namespace TarifasElectricas.Api.DTOs.Invoices.SimulateInvoice;

public record SimulateInvoiceResponseDto(
    Guid TariffId,
    Guid CompanyId,
    string CompanyName,
    decimal KwhConsumption,
    decimal ConsumptionCost,
    decimal TransportCost,
    decimal MarketingCost,
    decimal TotalCost,
    IReadOnlyList<InvoiceComponentDto> Components);
