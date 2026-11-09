namespace Backend.Domain.Result.Errors;

public static class InviteErrors
{
    public static Error Duplicate => 
        Error.Conflict("InviteResult.Duplicate", "Duplicate invitation");
    public static Error WrongRespondStatus => 
        Error.Validation("InviteResult.WrongRespondStatus", "Invitation status must be Accept or Reject");
}