namespace Backend.Domain;

public static class MatchErrors
{
    public static Error NotFound =>
        Error.NotFound("Match.NotFound", "Match wa not found.");
    public static Error InvalidDate =>
        Error.Validation("Match.InvalidDate", "Date of a finished match cannot be after today.");
}
