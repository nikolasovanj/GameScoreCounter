using System.Security.Claims;
using System.Text;
using Backend.Application.Interfaces.Services;
using Backend.Domain.Entities;
using Backend.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Infrastructure.Security;

public class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _o = options.Value;
    public string CreateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_o.SigningKey));
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _o.Issuer,
            Audience = _o.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_o.ExpiryMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity([
                new Claim("sub", user.UserId.ToString()),
                new Claim("username", user.Username)
            ])
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}