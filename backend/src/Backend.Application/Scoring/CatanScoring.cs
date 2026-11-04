using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Scoring;

public class CatanScoring : IScoringRule
{
    public GameType Game => GameType.Catan;

    public int? CalculateTotal(MatchResult result) => result.TotalScore;

    public Result Validate(MatchResult result)
    {
        if(result.TotalScore is null)
            return Result.Failure(MatchResultErrors.ScoreIsNull);
        if(result.TotalScore < 0)
            return Result.Failure(MatchResultErrors.ScoreIsNull);
        if(result.ScoreComponents.Count > 0)
            return Result.Failure(MatchResultErrors.TooManyComponents(Game));
        
        return Result.Success();
    }
}