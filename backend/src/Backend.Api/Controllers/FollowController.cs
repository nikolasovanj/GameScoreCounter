using Backend.Api.Contracts;
using Backend.Api.Extenstions;
using Backend.Application.Common;
using Backend.Application.DTOs;
using Backend.Application.Interfaces.Services;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/users/")]
[Authorize]
public class FollowController(IFollowService followService) : ControllerBase
{
    [HttpPut("{id}/follow")]
    public async Task<ActionResult<ApiResponse>> Follow([FromRoute]Guid id, CancellationToken ct)
    {
        var follower = User.GetUserId();
        var result = await followService.FollowAsync(follower, id, ct);
        return result.ToApiResult();
    }
    [HttpDelete("{id}/follow")]
    public async Task<ActionResult<ApiResponse>> Unfollow([FromRoute] Guid id, CancellationToken ct)
    {
        var follower = User.GetUserId();
        var result = await followService.UnfollowAsync(follower, id, ct);
        return result.ToApiResult();
    }

    [HttpGet("followers")]
    public async Task<ActionResult<ApiResponse<PagedResult<UserDTO>>>> GetFollowers([FromQuery] PageRequest request, CancellationToken ct) => 
        (await followService.GetFollowersAsync(User.GetUserId(), request, ct)).ToApiResult();

    [HttpGet("following")]
    public async Task<ActionResult<ApiResponse<PagedResult<UserDTO>>>> GetFollowing([FromQuery] PageRequest request, CancellationToken ct) =>
        (await followService.GetFollowingAsync(User.GetUserId(), request, ct)).ToApiResult();
}