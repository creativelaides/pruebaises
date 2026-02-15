using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Application.Interfaces;

namespace TarifasElectricas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtlController : ControllerBase
    {
        private readonly IEtlService _etlService;

        public EtlController(IEtlService etlService)
        {
            _etlService = etlService;
        }

        [HttpPost("ejecutar")]
        public async Task<IActionResult> EjecutarEtl()
        {
            var result = await _etlService.EjecutarEtlAsync();
            if (!result.Success)
            {
                return StatusCode(500, result.Message);
            }
            return Ok(result);
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetEtlLogs()
        {
            var logs = await _etlService.GetEtlLogsAsync();
            return Ok(logs);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var status = await _etlService.GetLatestEtlStatusAsync();
            if (status == null)
            {
                return NotFound("No ETL execution log found.");
            }
            return Ok(status);
        }
    }
}
