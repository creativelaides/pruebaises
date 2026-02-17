using Microsoft.AspNetCore.Identity;

namespace TarifasElectricas.Identity.Models;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string? Area { get; set; }

    public string FullName
    {
        get
        {
            var first = FirstName?.Trim();
            var last = LastName?.Trim();
            if (string.IsNullOrEmpty(first)) return last ?? string.Empty;
            if (string.IsNullOrEmpty(last)) return first;
            return $"{first} {last}";
        }
    }
}
