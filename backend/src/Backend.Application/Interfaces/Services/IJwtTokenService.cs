using Backend.Domain.Entities;

namespace Backend.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string CreateAccessToken(User user);
}