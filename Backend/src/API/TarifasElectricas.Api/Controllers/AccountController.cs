using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TarifasElectricas.Api.DTOs.Account.ChangePassword;
using TarifasElectricas.Api.DTOs.Account.GetProfile;
using TarifasElectricas.Api.DTOs.Account.UpdateProfile;
using TarifasElectricas.Identity.Models;

namespace TarifasElectricas.Api.Controllers;

[Tags("Account")]
[ApiController]
[Authorize]
[Route("api/account")]
public class AccountController(UserManager<AppUser> userManager) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager
        ?? throw new ArgumentNullException(nameof(userManager));

    [HttpGet("me")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetProfileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetProfileResponseDto>> Me()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var dto = new GetProfileResponseDto(
            user.Id,
            user.UserName,
            user.Email,
            user.FirstName,
            user.LastName,
            user.JobTitle,
            user.Area,
            await _userManager.GetRolesAsync(user));

        return Ok(dto);
    }

    [HttpPut("me")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetProfileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetProfileResponseDto>> UpdateMe([FromBody] UpdateProfileRequestDto request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        user.FirstName = request.FirstName?.Trim();
        user.LastName = request.LastName?.Trim();
        user.JobTitle = request.JobTitle?.Trim();
        user.Area = request.Area?.Trim();

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var newEmail = request.Email.Trim();
            user.Email = newEmail;
            user.UserName = newEmail;
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return BadRequest(string.Join("; ", updateResult.Errors.Select(e => e.Description)));

        var dto = new GetProfileResponseDto(
            user.Id,
            user.UserName,
            user.Email,
            user.FirstName,
            user.LastName,
            user.JobTitle,
            user.Area,
            await _userManager.GetRolesAsync(user));

        return Ok(dto);
    }

    [HttpPost("change-password")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            return BadRequest(string.Join("; ", result.Errors.Select(e => e.Description)));

        return Ok(new { message = "Contrasena actualizada." });
    }
}
