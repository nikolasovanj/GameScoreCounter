namespace Backend.Domain.Result.Errors;

public static class ScheduleErrors
{
    public static Error NotFound => 
        Error.NotFound("ScheduleResult.NotFound", "Match schedule was not found.");
    public static Error UnspecifiedTime => 
        Error.Validation("ScheduleResult.UnspecifiedTime", "Match Time must include a UTC offset.");
    public static Error PassedTime =>
        Error.Validation("ScheduleResult.PassedTime", "Match Time must be in the future.");
    public static Error NotAllowed =>
        Error.Unauthorized("ScheduleResult.NotAllowed", "Only the organizer can do that.");
    public static Error InvitedNotFound =>
        Error.Validation("ScheduleResult.InvitedNotFound", "One or more invited players dont exist.");
    public static Error Cancelled =>
        Error.Validation("ScheduleResult.Cancelled", "This schedule was cancelled.");
    public static Error Completed => 
        Error.Validation("Scheduleresult.Compelted", "This match is already completed.");
    public static Error OrganizerInvitationResponse => 
        Error.Validation("ScheduleResult.OrganizerInvitationResponse", "The organizer cannto change their response; cancel the schedule instead.");
    public static Error WrongGame => 
        Error.Validation("ScheduleResult.WrongGame", "The match is for a different game than the schedule.");
}