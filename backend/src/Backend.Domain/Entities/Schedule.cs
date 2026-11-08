using Backend.Domain.Enums;

namespace Backend.Domain.Entities;

public class Schedule
{
    public Guid ScheduleId { get; set;}
    public Guid OrganizerId { get; set;}
    public GameType Game { get; set;}
    public DateTime MatchTime { get; set;}
    public string? Location { get; set;}
    public ScheduleStatus Status { get; set;}
    public Guid? MatchId { get; set;}
    public DateTime CreatedAt { get; set;}
    public List<Invitation> Invitations { get; set; } = [];
}