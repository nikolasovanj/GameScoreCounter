using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;

namespace Backend.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> Login(LoginRequest request, CancellationToken ct = default);
    Task<AuthResponse> Register(RegisterRequest request, CancellationToken ct = default);
}
