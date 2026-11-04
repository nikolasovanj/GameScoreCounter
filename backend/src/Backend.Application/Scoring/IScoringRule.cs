using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Result;

namespace Backend.Application.Scoring;

public interface IScoringRule
{
    GameType Game { get; }
    Result Validate(MatchResult result);
    int? CalculateTotal(MatchResult result);    
}