using System.Security.Claims;
using LS_Projekt_ASP_2026.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LS_Projekt_ASP_2026.Services;

public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityAppUser, IdentityRole<int>>
{
    public AppUserClaimsPrincipalFactory(
        UserManager<IdentityAppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityAppUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var fullName = $"{user.Name} {user.Surname}".Trim();

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            identity.AddClaim(new Claim(AuthClaimTypes.FullName, fullName));
        }

        if (user.BusinessUserId.HasValue)
        {
            identity.AddClaim(new Claim(AuthClaimTypes.BusinessUserId, user.BusinessUserId.Value.ToString()));
        }

        return identity;
    }
}
