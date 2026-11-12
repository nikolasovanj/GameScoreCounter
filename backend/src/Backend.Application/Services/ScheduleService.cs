using Backend.Application.Common;
using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Application.Mappings;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;
using Microsoft.VisualBasic;

namespace Backend.Application.Services;

public class ScheduleService(
    IScheduleRepository scheduleRepository, 
    IMatchRepository matchRepository,
    IUserLookup userLookup,
    TimeProvider clock) : IScheduleService
{
    private readonly IScheduleRepository _schedules = scheduleRepository;
    private readonly IMatchRepository _matches = matchRepository;
    private readonly IUserLookup _users = userLookup;
    public async Task<Result> CancelAsync(Guid scheduleId, Guid organizedId, CancellationToken ct = default)
    {
        var scheduleResult = await LoadOrganizer(scheduleId, organizedId, ct);
        if(scheduleResult.IsFailure) return scheduleResult.Error;

        var schedule = scheduleResult.Value;

        if(schedule.Status == ScheduleStatus.CANCELLED) return Result.Success();
        if(schedule.Status == ScheduleStatus.COMPLETED) return ScheduleErrors.Completed;

        schedule.Status = ScheduleStatus.CANCELLED;
        if(!await _schedules.SaveChangesAsync(ct))
            return InviteErrors.Duplicate;
        return Result.Success();
    }

    public async Task<Result<ScheduleResponse>> CreateAsync(Guid organizerId, CreateScheduleRequest request, CancellationToken ct = default)
    {
        var matchTimeResult = NormalizeFutureTime(request.MatchTime);
        if(matchTimeResult.IsFailure)
            return matchTimeResult.Error;

        var invitees = (request.InvitedPlayers ?? [])
                        .Where(id => id != organizerId)
                        .Distinct()
                        .ToList();

        if(invitees.Count > 0 && !await _users.AllExistAsync(invitees, ct))
            return ScheduleErrors.InvitedNotFound;

        var now = Now();
        var schedule = new Schedule
        {
            ScheduleId = Guid.NewGuid(),
            OrganizerId = organizerId,
            Game = request.Game,
            MatchTime = matchTimeResult.Value,
            Location = request.Location?.Trim(),
            Status = ScheduleStatus.SCHEDULED,
            CreatedAt = now
        };
        schedule.Invitations.Add(new Invitation
        {
            InvitationId = Guid.NewGuid(),
            ScheduleId = schedule.ScheduleId,
            PlayerId = organizerId,
            Status = InvitationStatus.ACCEPTED,
            RespondedAt = now
        });

        foreach(var playerId in invitees)
            schedule.Invitations.Add(NewInvitation(schedule.ScheduleId, playerId));
        await _schedules.AddAsync(schedule, ct);
        return schedule.ToResponse();
    }

    public async Task<Result<ScheduleResponse>> GetAsync(Guid scheduleId, Guid requesterId, CancellationToken ct = default)
    {
        var scheduleResult = await Load(scheduleId, ct);
        if(scheduleResult.IsFailure)
            return scheduleResult.Error;
        
        var schedule = scheduleResult.Value;
        if(!schedule.Invitations.Any(i => i.PlayerId.Equals(requesterId)))
            return ScheduleErrors.NotFound;

        return schedule.ToResponse();
    }

    public async Task<Result<ScheduleResponse>> InviteAsync(Guid scheduleId, Guid organizerId, InviteRequest request, CancellationToken ct = default)
    {
        var scheduleResult = await LoadOrganizer(scheduleId, organizerId, ct);
        if(scheduleResult.IsFailure)
            return scheduleResult.Error;
        
        var schedule = scheduleResult.Value;
        var check = EnsureOpen(schedule);
        if(check.IsFailure) return check.Error;

        var existing = schedule.Invitations.Select(i => i.PlayerId).ToHashSet();
        var newIds = request.PlayerIds.Distinct().Where(id => !existing.Contains(id)).ToList();

        if(newIds.Count > 0)
        {
            if(!await _users.AllExistAsync(newIds, ct))
                return ScheduleErrors.InvitedNotFound;
            
            foreach(var playerId in newIds)
                schedule.Invitations.Add(NewInvitation(schedule.ScheduleId, playerId));

            if(!await _schedules.SaveChangesAsync(ct))
                return InviteErrors.Duplicate;
        }

        return schedule.ToResponse();
    }

    public async Task<Result<ScheduleResponse>> LinkMatchAsync(Guid scheduleId, Guid organizerId, Guid matchId, CancellationToken ct = default)
    {
        var scheduleResult = await LoadOrganizer(scheduleId, organizerId, ct);
        if(scheduleResult.IsFailure) return scheduleResult.Error;

        var schedule = scheduleResult.Value;
        if(schedule.Status == ScheduleStatus.COMPLETED && schedule.MatchId.Equals(matchId))
            return schedule.ToResponse();
        
        if(schedule.Status != ScheduleStatus.SCHEDULED)
            return ScheduleErrors.Cancelled;
        
        var match = await _matches.GetByMatchIdAsync(matchId, ct);
        if(match is null) return MatchErrors.NotFound;

        if(match.Game != schedule.Game)
            return ScheduleErrors.WrongGame;
        
        schedule.MatchId = matchId;
        schedule.Status = ScheduleStatus.COMPLETED;
        if(!await _schedules.SaveChangesAsync(ct)) return InviteErrors.Duplicate;
        return schedule.ToResponse();
    }

    public async Task<Result<PagedResult<ScheduleResponse>>> ListMineAsync(Guid playerId, PageRequest request, CancellationToken ct = default) => 
        (await _schedules.ListForPlayerAsync(playerId, Now(), request, ct)).Map(s => s.ToResponse());

    public async Task<Result<ScheduleResponse>> RescheduleAsync(Guid scheduleId, Guid organizerId, RescheduleRequest request, CancellationToken ct = default)
    {
        var scheduleResult = await LoadOrganizer(scheduleId, organizerId, ct);
        if(scheduleResult.IsFailure) 
            return scheduleResult.Error;
        
        var schedule = scheduleResult.Value;

        var check = EnsureOpen(schedule);
        if(check.IsFailure) return check.Error;

        var timeResult = NormalizeFutureTime(request.MatchTime);
        if(timeResult.IsFailure) return timeResult.Error;

        schedule.MatchTime = timeResult.Value;
        schedule.Location = request.Location?.Trim();

        foreach(var inv in schedule.Invitations.Where(i => !i.PlayerId.Equals(organizerId) && i.Status != InvitationStatus.PENDING))
        {
            inv.Status = InvitationStatus.PENDING;
            inv.RespondedAt = null;
        }

        if(!await _schedules.SaveChangesAsync(ct)) return InviteErrors.Duplicate;
        return schedule.ToResponse();

    }

    public async Task<Result<ScheduleResponse>> RespondAsync(Guid scheduleId, Guid playerId, InvitationStatus status, CancellationToken ct = default)
    {
        if(status is not (InvitationStatus.ACCEPTED or InvitationStatus.REJECTED))
            return InviteErrors.WrongRespondStatus;
        
        var scheduleResult = await Load(scheduleId, ct);
        if(scheduleResult.IsFailure)
            return scheduleResult.Error;
        var schedule = scheduleResult.Value;
        var invitation = schedule.Invitations.FirstOrDefault(i => i.PlayerId.Equals(playerId));
        if(invitation is null) 
            return ScheduleErrors.NotFound;
        
        if(playerId.Equals(schedule.OrganizerId))
            return ScheduleErrors.OrganizerInvitationResponse;
        
        var check = EnsureOpen(schedule);
        if(check.IsFailure) return check.Error;

        if(invitation.Status != status)
        {
            invitation.Status = status;
            invitation.RespondedAt = Now();
            if(!await _schedules.SaveChangesAsync(ct))
                return InviteErrors.Duplicate;
        }

        return schedule.ToResponse();
    }

    #region Helpers
    private DateTime Now() => clock.GetUtcNow().UtcDateTime;

    private Result<DateTime> NormalizeFutureTime(DateTime time)
    {
        if(time.Kind == DateTimeKind.Unspecified)
            return ScheduleErrors.UnspecifiedTime;
        
        var utc = time.ToUniversalTime();
        if(utc < Now())
            return ScheduleErrors.PassedTime;
        
        return utc;
    }

    private static Invitation NewInvitation(Guid scheduleId, Guid PlayerId) => new()
    {
        InvitationId = Guid.NewGuid(),
        ScheduleId = scheduleId,
        PlayerId = PlayerId,
        Status = InvitationStatus.PENDING

    };

    private async Task<Result<Schedule>> Load(Guid id, CancellationToken ct) => 
        (await _schedules.GetByIdAsync(id, ct)).OnError(ScheduleErrors.NotFound);
    private async Task<Result<Schedule>> LoadOrganizer(Guid id, Guid organizerId, CancellationToken ct)
    {
        var scheduleResult = await Load(id, ct);
        if(scheduleResult.IsFailure)
            return scheduleResult.Error;
        
        var schedule = scheduleResult.Value;
        if(!schedule.OrganizerId.Equals(organizerId))
            return ScheduleErrors.NotAllowed;

        return schedule;
    }

    private Result EnsureOpen(Schedule s)
    {
        if(s.Status == ScheduleStatus.CANCELLED) 
            return ScheduleErrors.Cancelled;
        if(s.Status == ScheduleStatus.COMPLETED)
            return ScheduleErrors.Completed;
        if(s.MatchTime <= Now())
            return ScheduleErrors.PassedTime;
        
        return Result.Success();
    }

    #endregion
}