using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Scoring;

public class SeaSaltScoring : IScoringRule
{
    public GameType Game => GameType.SeaSaltAndPaper;

    public int? CalculateTotal(MatchResult result) => result.ScoreComponents.Sum(c => c.Points);

    public Result Validate(MatchResult result)
    {
        if(result.ScoreComponents.Count <= 0)
            return Result.Failure(MatchResultErrors.NoComponents);
        if(result.ScoreComponents.Select(c => c.Order).Distinct().Count() != result.ScoreComponents.Count)
            return Result.Failure(MatchResultErrors.ComponentsAreNotUnique);
        if(result.TotalScore is not null)
            return Result.Failure(MatchResultErrors.ScoreAlreadyCalculated);

        return Result.Success();
    }
}