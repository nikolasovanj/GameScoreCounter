using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Api.Extenstions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }
}