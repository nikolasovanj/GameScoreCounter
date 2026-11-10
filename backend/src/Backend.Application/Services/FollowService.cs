using Backend.Application.Common;
using Backend.Application.DTOs;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Application.Mappings;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Services;

public class FollowService(IFollowRepository followRepository, IUserLookup userLookup) : IFollowService
{
    private readonly IFollowRepository _follows = followRepository;
    private readonly IUserLookup _users = userLookup;
    public async Task<Result> FollowAsync(Guid me, Guid target, CancellationToken ct = default)
    {
        if(me.Equals(target)) 
            return Result.Failure(FollowErrors.SelfFollow);
        if(!await _users.ExistsAsync(target, ct)) 
            return Result.Failure(UserErrors.NotFound);

        await _follows.AddIfMissingAsync(me, target, ct);
        return Result.Success();
    }

    public async Task<Result<PagedResult<UserDTO>>> GetFollowersAsync(Guid userId, PageRequest request, CancellationToken ct = default) => 
        (await _follows.GetFollowersAsync(userId, request, ct)).Map(UserMapper.ToDto);

    public async Task<Result<PagedResult<UserDTO>>> GetFollowingAsync(Guid userId, PageRequest request, CancellationToken ct = default) => 
        (await _follows.GetFollowingAsync(userId, request, ct)).Map(UserMapper.ToDto);

    public async Task<Result> UnfollowAsync(Guid me, Guid target, CancellationToken ct = default)
    {
        await _follows.DeleteAsync(me, target, ct);
        return Result.Success();
    }
}