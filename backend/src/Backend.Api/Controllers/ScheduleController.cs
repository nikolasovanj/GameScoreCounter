using Backend.Api.Extenstions;
using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Services;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduleController(IScheduleService scheduleService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ScheduleResponse>> Create([FromBody] CreateScheduleRequest request, CancellationToken ct) => 
        (await scheduleService.CreateAsync(User.GetUserId(), request, ct)).ToApiResult();

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ScheduleResponse>>> GetForUser(CancellationToken ct) => 
        (await scheduleService.ListMineAsync(User.GetUserId(), ct)).ToApiResult();
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleResponse>> GetById([FromRoute]Guid id, CancellationToken ct) => 
        (await scheduleService.GetAsync(id, User.GetUserId(), ct)).ToApiResult();

    [HttpPost("{id}/invitations")]
    public async Task<ActionResult<ScheduleResponse>> InvitePlayers([FromRoute] Guid id, [FromBody]InviteRequest request, CancellationToken ct) => 
        (await scheduleService.InviteAsync(id, User.GetUserId(), request, ct)).ToApiResult();

    [HttpPut("{id}/invitation/me")]
    public async Task<ActionResult<ScheduleResponse>> Respond([FromRoute] Guid id, [FromBody] InvitationStatus status, CancellationToken ct) => 
        (await scheduleService.RespondAsync(id, User.GetUserId(), status, ct)).ToApiResult();

    [HttpPut("{id}")]
    public async Task<ActionResult<ScheduleResponse>> Reschedule([FromRoute] Guid id, [FromBody] RescheduleRequest request, CancellationToken ct) =>
        (await scheduleService.RescheduleAsync(id, User.GetUserId(), request, ct)).ToApiResult();
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Cancel([FromRoute] Guid id, CancellationToken ct) =>
        (await scheduleService.CancelAsync(id, User.GetUserId(), ct)).ToApiResult();

    [HttpPut("{id}/match")]
    public async Task<ActionResult<ScheduleController>> LinkMatch([FromRoute] Guid id, [FromBody] Guid matchId, CancellationToken ct) => 
        (await scheduleService.LinkMatchAsync(id, User.GetUserId(), matchId, ct)).ToApiResult();
}