using Backend.Application.Common;
using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Domain.Enums;
using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Services;

public interface IScheduleService
{
    Task<Result<ScheduleResponse>> CreateAsync(Guid organizerId, CreateScheduleRequest request, CancellationToken ct = default);
    Task<Result<ScheduleResponse>> GetAsync(Guid scheduleId, Guid requesterId, CancellationToken ct = default);
    Task<Result<PagedResult<ScheduleResponse>>> ListMineAsync(Guid playerId, PageRequest request, CancellationToken ct = default);
    Task<Result<ScheduleResponse>> InviteAsync(Guid scheduleId, Guid organizerId, InviteRequest request, CancellationToken ct = default);
    Task<Result<ScheduleResponse>> RespondAsync(Guid scheduleId, Guid playerId, InvitationStatus status, CancellationToken ct = default);
    Task<Result<ScheduleResponse>> RescheduleAsync(Guid scheduleId, Guid organizerId, RescheduleRequest request, CancellationToken ct = default);
    Task<Result> CancelAsync(Guid scheduleId, Guid organizedId, CancellationToken ct = default);
    Task<Result<ScheduleResponse>> LinkMatchAsync(Guid scheduleId, Guid organizerId, Guid matchId, CancellationToken ct = default);
}