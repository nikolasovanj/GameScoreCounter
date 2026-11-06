using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Application.Mappings;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Services;

public class AuthService(
    IAuthRepository authRepository, 
    IPasswordService passwordService, 
    IJwtTokenService jwtTokenService
    ) : IAuthService
{
    private readonly IAuthRepository _repo = authRepository;
    private readonly IPasswordService _hasher = passwordService;
    private readonly IJwtTokenService _jwt = jwtTokenService;
    public async Task<Result<AuthResponse>> Login(LoginRequest request, CancellationToken ct = default)
    { 
        var user = await _repo.GetExisting(request.Username, ct);
        if(user is null || !_hasher.Verify(user.PasswordHash, request.Password, out var needsRehash))
            return UserErrors.InvalidCredentials;

        if(needsRehash)
        {
            user = await _repo.Rehash(user, _hasher.Hash(request.Password), ct);
            if(user is null)
                return UserErrors.InvalidCredentials;
        }

        return new AuthResponse(_jwt.CreateAccessToken(user));
    }

    public async Task<Result<AuthResponse>> Register(RegisterRequest request, CancellationToken ct = default)
    {
        if(!request.Password.Equals(request.PasswordRepeat))
            return UserErrors.PasswordsDontMatch;
        if(await _repo.GetExisting(request.Username, ct) is not null)
            return UserErrors.UsernameAlreadyExists;

        var hash = _hasher.Hash(request.Password);

        var user = await _repo.Register(request.ToEntity(hash), ct);
        if(user is null)
            return UserErrors.NotFound;

        return new AuthResponse(_jwt.CreateAccessToken(user));
    }
}
