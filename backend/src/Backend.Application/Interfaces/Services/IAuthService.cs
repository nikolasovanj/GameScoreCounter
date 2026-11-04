using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> Login(LoginRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> Register(CreateUserRequest request, CancellationToken ct = default);
}
