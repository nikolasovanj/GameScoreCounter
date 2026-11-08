using Backend.Domain.Enums;

namespace Backend.Domain.Entities;

public class Invitation
{
    public Guid InvitationId { get; set;}
    public Guid PlayerId { get; set;}    
    public Guid ScheduleId { get; set; }
    public InvitationStatus Status { get; set;} = InvitationStatus.PENDING;
    public DateTime? RespondedAt { get; set;}
}