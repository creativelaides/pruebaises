using System.Collections.Generic;

namespace TarifasElectricas.Api.DTOs.Account.GetProfile;

public sealed record GetProfileResponseDto(
    string Id,
    string? UserName,
    string? Email,
    string? FirstName,
    string? LastName,
    string? JobTitle,
    string? Area,
    IEnumerable<string> Roles);
