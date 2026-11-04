using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Application.Mappings;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _repo = userRepository;

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if(!await _repo.DeleteAsync(id, ct))
            return Result.Failure(MatchErrors.NotFound);
        return Result.Success();
    }

    public async Task<Result<UserDTO?>> GetByUserIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _repo.GetByUserIdAsync(id, ct);
        if(user is null)
            return Result<UserDTO?>.Failure(UserErrors.NotFound);
        return Result<UserDTO?>.Success(user.ToDto());

    }
}
