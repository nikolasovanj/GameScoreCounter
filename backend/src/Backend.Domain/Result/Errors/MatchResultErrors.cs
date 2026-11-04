using Backend.Domain.Enums;

namespace Backend.Domain.Result.Errors;

public static class MatchResultErrors
{   
    public static Error NotFound => 
        Error.NotFound("MatchResult.NotFound", "Match result was not found.");
    public static Error ScoreIsNull => 
        Error.Validation("MatchResult.ScoreIsNull", "Match result is missing score total."); 
    public static Error ScoreIsNegative => 
        Error.Validation("MatchResult.ScoreIsNegative", "Match result is negative.");
    public static Error TooManyComponents(GameType game) => 
        Error.Validation("MatchResult.TooManyComponents", $"Match result for {game} has too many components");
    public static Error NoComponents => 
        Error.Validation("MatchResult.NoComponents", "Match result has no components.");
    public static Error ComponentsAreNotUnique => 
        Error.Validation("MatchResult.ComponentsNotUnique", "Match result components are not unique.");
    public static Error ScoreAlreadyCalculated => 
        Error.Validation("MatchResult.ScoreCalculated", "Match results score is already calculated.");   
}
