namespace Backend.Domain;

public static class MatchResultErrors
{   
    public static Error NotFound => 
        Error.NotFound("MatchResult.NotFound", "Match result was not found.");
}
