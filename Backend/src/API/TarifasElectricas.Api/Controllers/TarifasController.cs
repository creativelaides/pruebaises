using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Application.DTOs;
using TarifasElectricas.Application.Interfaces;

namespace TarifasElectricas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarifasController : ControllerBase
    {
        private readonly ITarifaService _tarifaService;

        public TarifasController(ITarifaService tarifaService)
        {
            _tarifaService = tarifaService;
        }

        [HttpGet("actual")]
        public async Task<ActionResult<TarifaDTO>> GetTarifaActual()
        {
            var tarifa = await _tarifaService.GetTarifaActualAsync();
            if (tarifa == null)
            {
                return NotFound();
            }
            return Ok(tarifa);
        }

        [HttpGet("componentes")]
        public async Task<ActionResult<IEnumerable<ComponenteEducacionDTO>>> GetComponentesEducativos()
        {
            var componentes = await _tarifaService.GetComponentesEducativosAsync();
            return Ok(componentes);
        }
    }
}
