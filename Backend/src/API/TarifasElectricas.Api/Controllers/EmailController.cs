using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Application.Contracts.Services;
using TarifasElectricas.Identity.Models;

namespace TarifasElectricas.Api.Controllers;

[Tags("Email")]
[ApiController]
[Route("api/email")]
[Authorize(Roles = AppRoles.Admin)]
public class EmailController(IEmailService emailService, IWebHostEnvironment environment) : ControllerBase
{
    private readonly IEmailService _emailService = emailService 
    ?? throw new ArgumentNullException(nameof(emailService));
    private readonly IWebHostEnvironment _environment = environment
    ?? throw new ArgumentNullException(nameof(environment));

    /// <summary>
    /// Envía un correo de prueba (solo Development).
    /// </summary>
    [HttpPost("test")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendTestEmail([FromQuery] string to)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        if (string.IsNullOrWhiteSpace(to))
            return BadRequest("El parámetro 'to' es requerido.");

        var message = new EmailMessage(
            to,
            "Prueba de correo - PruebaIses",
            "Hola,\n\nEste es un correo de prueba enviado desde la API.\n\nEquipo PruebaIses",
            IsHtml: false);

        await _emailService.SendAsync(message);

        return Ok(new { message = "Correo enviado." });
    }
}
