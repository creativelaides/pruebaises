using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Api.DTOs.Tariffs.CreateTariff;
using TarifasElectricas.Api.DTOs.Tariffs.DeleteTariff;
using TarifasElectricas.Api.DTOs.Tariffs.GetAllTariffs;
using TarifasElectricas.Api.DTOs.Tariffs.GetLatestTariff;
using TarifasElectricas.Api.DTOs.Tariffs.GetTariffById;
using TarifasElectricas.Api.DTOs.Tariffs.GetTariffByPeriod;
using TarifasElectricas.Api.DTOs.Tariffs.UpdateTariff;
using TarifasElectricas.Application.Exceptions;
using TarifasElectricas.Application.UseCases.Commands.CreateTariff;
using TarifasElectricas.Application.UseCases.Commands.DeleteTariff;
using TarifasElectricas.Application.UseCases.Commands.UpdateTariff;
using TarifasElectricas.Application.UseCases.Queries.GetAllTariffs;
using TarifasElectricas.Application.UseCases.Queries.GetLatestTariff;
using TarifasElectricas.Application.UseCases.Queries.GetTariffById;
using TarifasElectricas.Application.UseCases.Queries.GetTariffByPeriod;
using TarifasElectricas.Identity.Models;

namespace TarifasElectricas.Api.Controllers;

[Tags("Tariffs")]
[ApiController]
[Authorize(Policy = AppPolicies.CanQueryTariffs)]
[Route("api/tariffs")]
public class TariffsController(
    CreateTariffCommandHandler createHandler,
    UpdateTariffCommandHandler updateHandler,
    DeleteTariffCommandHandler deleteHandler,
    GetTariffByIdQueryHandler getByIdHandler,
    GetTariffByPeriodQueryHandler getByPeriodHandler,
    GetLatestTariffQueryHandler getLatestHandler,
    GetAllTariffsQueryHandler getAllHandler) : ControllerBase
{
    private readonly CreateTariffCommandHandler _createHandler = createHandler
    ?? throw new ArgumentNullException(nameof(createHandler));
    private readonly UpdateTariffCommandHandler _updateHandler = updateHandler
    ?? throw new ArgumentNullException(nameof(updateHandler));
    private readonly DeleteTariffCommandHandler _deleteHandler = deleteHandler
    ?? throw new ArgumentNullException(nameof(deleteHandler));
    private readonly GetTariffByIdQueryHandler _getByIdHandler = getByIdHandler
    ?? throw new ArgumentNullException(nameof(getByIdHandler));
    private readonly GetTariffByPeriodQueryHandler _getByPeriodHandler = getByPeriodHandler
    ?? throw new ArgumentNullException(nameof(getByPeriodHandler));
    private readonly GetLatestTariffQueryHandler _getLatestHandler = getLatestHandler
    ?? throw new ArgumentNullException(nameof(getLatestHandler));
    private readonly GetAllTariffsQueryHandler _getAllHandler = getAllHandler
    ?? throw new ArgumentNullException(nameof(getAllHandler));


    /// <summary>
    /// Crea una nueva tarifa eléctrica.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CreateTariffResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateTariffResponseDto>> Create([FromBody] CreateTariffRequest request)
    {
        try
        {
            var command = new CreateTariffCommand(
                request.Year,
                request.Period,
                request.Level,
                request.TariffOperator,
                request.CompanyId,
                request.TotalCu,
                request.PurchaseCostG,
                request.ChargeTransportStnTm,
                request.ChargeTransportSdlDm,
                request.MarketingMargin,
                request.CostLossesPr,
                request.RestrictionsRm,
                request.Cot,
                request.CfmjGfact);

            var result = await _createHandler.Handle(command);
            var dto = new CreateTariffResponseDto(
                result.Id,
                result.Year,
                result.Period,
                result.Level,
                result.TariffOperator,
                result.CompanyId,
                result.TotalCosts,
                result.CreatedAt);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
        catch (ApplicationCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Actualiza los costos de una tarifa existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = AppRoles.Admin)]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UpdateTariffResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateTariffResponseDto>> Update(Guid id, [FromBody] UpdateTariffRequest request)
    {
        try
        {
            var command = new UpdateTariffCommand(
                id,
                request.TotalCu,
                request.PurchaseCostG,
                request.ChargeTransportStnTm,
                request.ChargeTransportSdlDm,
                request.MarketingMargin,
                request.CostLossesPr,
                request.RestrictionsRm,
                request.Cot,
                request.CfmjGfact);

            var result = await _updateHandler.Handle(command);
            var dto = new UpdateTariffResponseDto(
                result.Id,
                result.Year,
                result.Period,
                result.Level,
                result.TariffOperator,
                result.CompanyId,
                result.TotalCosts,
                result.UpdatedAt);

            return Ok(dto);
        }
        catch (ApplicationCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina una tarifa por ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = AppRoles.Admin)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DeleteTariffResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteTariffResponseDto>> Delete(Guid id)
    {
        try
        {
            var command = new DeleteTariffCommand(id);
            var result = await _deleteHandler.Handle(command);
            var dto = new DeleteTariffResponseDto(result.Id, result.Success, result.Message);
            return Ok(dto);
        }
        catch (ApplicationCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene una tarifa por ID.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetTariffByIdResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetTariffByIdResponseDto>> GetById(Guid id)
    {
        try
        {
            var result = await _getByIdHandler.Handle(new GetTariffByIdQuery(id));
            var dto = new GetTariffByIdResponseDto(
                result.Id,
                result.Year,
                result.Period,
                result.Level,
                result.TariffOperator,
                result.CompanyId,
                result.TotalCosts,
                result.CreatedAt);
            return Ok(dto);
        }
        catch (ApplicationCaseException ex)
        {
            return IsNotFound(ex) ? NotFound(ex.Message) : BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene tarifas por año y período.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("by-period")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetTariffByPeriodResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetTariffByPeriodResponseDto>> GetByPeriod(
        [FromQuery] int year,
        [FromQuery] string period)
    {
        try
        {
            var result = await _getByPeriodHandler.Handle(new GetTariffByPeriodQuery(year, period));
            var dto = new GetTariffByPeriodResponseDto(
                result.Tariffs.Select(t => new GetTariffByPeriodResponseDto.TariffItem(
                    t.Id,
                    t.Year,
                    t.Period,
                    t.Level,
                    t.TariffOperator,
                    t.CompanyId,
                    t.TotalCosts,
                    t.CreatedAt)));
            return Ok(dto);
        }
        catch (ApplicationCaseException ex)
        {
            return IsNotFound(ex) ? NotFound(ex.Message) : BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene la tarifa más reciente.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("latest")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetLatestTariffResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetLatestTariffResponseDto>> GetLatest()
    {
        try
        {
            var result = await _getLatestHandler.Handle(new GetLatestTariffQuery());
            var dto = new GetLatestTariffResponseDto(
                result.Id,
                result.Year,
                result.Period,
                result.Level,
                result.TariffOperator,
                result.CompanyId,
                result.TotalCosts,
                result.CreatedAt);
            return Ok(dto);
        }
        catch (ApplicationCaseException ex)
        {
            return IsNotFound(ex) ? NotFound(ex.Message) : BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Lista tarifas con paginación básica.
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetAllTariffsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetAllTariffsResponseDto>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var result = await _getAllHandler.Handle(new GetAllTariffsQuery(page, pageSize));
            var items = result.Tariffs.Select(t =>
                new GetAllTariffsResponseDto.TariffItem(
                    t.Id,
                    t.Year,
                    t.Period,
                    t.Level,
                    t.TariffOperator,
                    t.CompanyId,
                    t.TotalCosts,
                    t.CreatedAt));

            return Ok(new GetAllTariffsResponseDto(items));
        }
        catch (ApplicationCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static bool IsNotFound(ApplicationCaseException ex) =>
        ex.Message.Contains("no encontrada", StringComparison.OrdinalIgnoreCase) ||
        ex.Message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
        ex.Message.Contains("no hay tarifas", StringComparison.OrdinalIgnoreCase);
}
