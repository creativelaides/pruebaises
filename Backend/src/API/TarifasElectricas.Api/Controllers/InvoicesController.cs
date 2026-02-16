using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Api.DTOs.Invoices.SimulateInvoice;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Queries.SimulateInvoice;

namespace TarifasElectricas.Api.Controllers;

[Tags("Invoices")]
[ApiController]
[Route("api/invoices")]
public class InvoicesController(SimulateInvoiceQueryHandler simulateHandler) : ControllerBase
{
    private readonly SimulateInvoiceQueryHandler _simulateHandler = simulateHandler
    ?? throw new ArgumentNullException(nameof(simulateHandler));


    /// <summary>
    /// Simula una factura a partir de una tarifa y consumo en kWh.
    /// </summary>
    [HttpPost("simulate")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SimulateInvoiceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SimulateInvoiceResponseDto>> Simulate([FromBody] SimulateInvoiceRequest request)
    {
        try
        {
            var query = new SimulateInvoiceQuery(request.TariffId, request.KwhConsumption);
            var result = await _simulateHandler.Handle(query);

            var components = result.Components
                .Select(c => new InvoiceComponentDto(c.Name, c.Value, c.Explanation))
                .ToList();

            var dto = new SimulateInvoiceResponseDto(
                result.TariffId,
                result.CompanyId,
                result.CompanyName,
                result.KwhConsumption,
                result.ConsumptionCost,
                result.TransportCost,
                result.MarketingCost,
                result.TotalCost,
                components);

            return Ok(dto);
        }
        catch (ApplicationCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
