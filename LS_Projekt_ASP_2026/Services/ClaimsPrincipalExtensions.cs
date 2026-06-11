using System.Security.Claims;

namespace LS_Projekt_ASP_2026.Services;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(AuthClaimTypes.BusinessUserId)
            ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }
}
