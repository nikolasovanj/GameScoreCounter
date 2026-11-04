using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Application.Mappings;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    private readonly IAuthRepository _repo = authRepository;
    public async Task<Result<AuthResponse>> Login(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _repo.Login(request, ct);
        if(user is null)
            return Result<AuthResponse>.Failure(UserErrors.NotFound);

        var token = user.Username;
        return Result<AuthResponse>.Success(
            new AuthResponse(token)
        );
    }

    public async Task<Result<AuthResponse>> Register(CreateUserRequest request, CancellationToken ct = default)
    {
        var user = await _repo.Register(request.ToEntity(), ct);
        if(user is null)
            return Result<AuthResponse>.Failure(UserErrors.NotFound);

        var token = user.Username;
        return Result<AuthResponse>.Success(
            new AuthResponse(token)
        );
    }
}
