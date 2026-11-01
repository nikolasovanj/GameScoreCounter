using Backend.Application.DTOs.Requests;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;

namespace Backend.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    public Task<User?> Login(LoginRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> Register(CreateUserRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
