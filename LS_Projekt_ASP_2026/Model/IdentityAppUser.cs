using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LS_Projekt_ASP_2026.Model;

public class IdentityAppUser : IdentityUser<int>
{
    public string Name { get; set; } = "";

    public string Surname { get; set; } = "";

    [Required]
    [StringLength(11, MinimumLength = 11)]
    [RegularExpression("^[0-9]*$")]
    public string OIB { get; set; } = "";

    [Required]
    [StringLength(13, MinimumLength = 13)]
    [RegularExpression("^[0-9]*$")]
    public string JMBG { get; set; } = "";

    public DateTime? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? BusinessUserId { get; set; }
}
