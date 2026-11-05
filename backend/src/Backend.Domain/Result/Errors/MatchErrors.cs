using Backend.Domain.Enums;

namespace Backend.Domain.Result.Errors;

public static class MatchErrors
{
    public static Error NotFound =>
        Error.NotFound("Match.NotFound", "Match wa not found.");
    public static Error InvalidDate =>
        Error.Validation("Match.InvalidDate", "Date of a finished match cannot be after today.");
    public static Error NoRuleForGame(GameType game) => 
        Error.Validation("Match.NoRuleForGame", $"There is no rule registerd for game {game}.");
    public static Error NoResults => 
        Error.Validation("Match.NoResults", "A match must have atleast one result.");
    public static Error PlayersNotUnique => 
        Error.Validation("Match.PlayersNotUnique", "Match players must be unique.");
}
