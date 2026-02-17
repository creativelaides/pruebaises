namespace TarifasElectricas.Api.DTOs.Account.ChangePassword;

public sealed record ChangePasswordRequestDto(
    string CurrentPassword,
    string NewPassword);
