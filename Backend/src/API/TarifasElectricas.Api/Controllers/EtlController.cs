using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Api.DTOs.Etl.ExecuteEtl;
using TarifasElectricas.Application.Contracts.Services;
using TarifasElectricas.Identity.Models;

namespace TarifasElectricas.Api.Controllers;

[Tags("ETL")]
[ApiController]
[Authorize(Policy = AppPolicies.CanRunEtl)]
[Route("api/etl")]
public class EtlController(IEtlService etlService) : ControllerBase
{
    private readonly IEtlService _etlService = etlService 
    ?? throw new ArgumentNullException(nameof(etlService));


    /// <summary>
    /// Ejecuta el proceso ETL desde datos.gov.co y retorna un resumen.
    /// </summary>
    [HttpPost("run")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ExecuteEtlResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExecuteEtlResponseDto>> Run()
    {
        var result = await _etlService.ExecuteAsync();
        var dto = new ExecuteEtlResponseDto(
            result.Success,
            result.ProcessedCount,
            result.ErrorCount,
            result.Message,
            result.DurationSeconds,
            result.ExecutionDate);

        return Ok(dto);
    }
}
