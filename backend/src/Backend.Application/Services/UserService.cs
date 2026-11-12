using Backend.Application.Common;
using Backend.Application.DTOs;
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
            return MatchErrors.NotFound;
        return Result.Success();
    }

    public async Task<Result<PagedResult<UserDTO>>> GetAllAsync(PageRequest request, CancellationToken ct = default) => 
        (await _repo.GetAllAsync(request, ct)).Map(u => u.ToDto());

    public async Task<Result<UserDTO>> GetByUsenameAsync(string username, CancellationToken ct = default)
    {
        var user = await _repo.GetByUsernameAsync(username, ct);
        if(user is null)
            return UserErrors.NotFound;
        
        return user.ToDto();
    }

    public async Task<Result<UserDTO>> GetByUserIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _repo.GetByUserIdAsync(id, ct);
        if(user is null)
            return UserErrors.NotFound;
        
        return user.ToDto();
    }
}
