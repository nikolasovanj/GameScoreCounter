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
    public async Task<ActionResult<ApiResponse>> Follow([FromRoute] Guid id, CancellationToken ct) => 
        (await followService.FollowAsync(User.GetUserId(), id, ct)).ToNoContentResult();

    [HttpDelete("{id}/follow")]
    public async Task<ActionResult<ApiResponse>> Unfollow([FromRoute] Guid id, CancellationToken ct) =>
        (await followService.UnfollowAsync(User.GetUserId(), id, ct)).ToNoContentResult();

    [HttpGet("followers")]
    public async Task<ActionResult<ApiResponse<PagedResult<UserDTO>>>> GetFollowers([FromQuery] PageRequest request, CancellationToken ct) => 
        (await followService.GetFollowersAsync(User.GetUserId(), request, ct)).ToApiResult();

    [HttpGet("following")]
    public async Task<ActionResult<ApiResponse<PagedResult<UserDTO>>>> GetFollowing([FromQuery] PageRequest request, CancellationToken ct) =>
        (await followService.GetFollowingAsync(User.GetUserId(), request, ct)).ToApiResult();
}