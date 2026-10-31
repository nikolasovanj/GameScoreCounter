using Backend.Application.DTOs.Requests;
using Backend.Domain.Entities;

namespace Backend.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User?> Login(LoginRequest request, CancellationToken ct = default);
    Task<User?> Register(CreateUserRequest request, CancellationToken ct = default);
}
