using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Application.DTOs;

namespace TarifasElectricas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        // For now, the logic will be simple and contained here.
        // In a fuller implementation, this would call a dedicated service.

        [HttpPost("simular")]
        public async Task<ActionResult<FacturaSimuladaDTO>> SimularFactura([FromBody] int consumoKwh)
        {
            if (consumoKwh <= 0)
            {
                return BadRequest("El consumo debe ser mayor a cero.");
            }

            // Dummy tariff for simulation
            var tarifaActual = 750.5m;

            var valorTotal = consumoKwh * tarifaActual;

            var result = new FacturaSimuladaDTO
            {
                ConsumoKwh = consumoKwh,
                ValorTotalPagar = valorTotal,
                CuTotalCalculado = tarifaActual,
                // Populate other fields as needed for the simulation
            };

            return Ok(await Task.FromResult(result));
        }
    }
}
