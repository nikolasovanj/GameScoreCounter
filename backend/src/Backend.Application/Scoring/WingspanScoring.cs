using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Scoring;

public class WingspanScoring : IScoringRule
{
    public GameType Game => GameType.Wingspan;

    public int? CalculateTotal(MatchResult result) => result.ScoreComponents.Sum(c => c.Points);

    public Result Validate(MatchResult result)
    {
        if(result.ScoreComponents.Count <6)
            return Result.Failure(MatchResultErrors.TooLittleComponents(Game));
        if(result.ScoreComponents.Count > 7)
            return Result.Failure(MatchResultErrors.TooManyComponents(Game));
        if(result.ScoreComponents.Select(c => c.Order).Distinct().Count() != result.ScoreComponents.Count)
            return Result.Failure(MatchResultErrors.ComponentsAreNotUnique);
        if(result.TotalScore is not null)
            return Result.Failure(MatchResultErrors.ScoreAlreadyCalculated);
        
        return Result.Success();
    }
}