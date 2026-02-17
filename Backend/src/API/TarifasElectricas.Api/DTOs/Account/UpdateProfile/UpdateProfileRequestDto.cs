namespace TarifasElectricas.Api.DTOs.Account.UpdateProfile;

public sealed record UpdateProfileRequestDto(
    string? FirstName,
    string? LastName,
    string? JobTitle,
    string? Area,
    string? Email);
