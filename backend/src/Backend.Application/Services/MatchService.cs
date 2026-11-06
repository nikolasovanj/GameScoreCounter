using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Application.Mappings;
using Backend.Application.Scoring;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Result;
using Backend.Domain.Result.Errors;

namespace Backend.Application.Services;

public class MatchService(IMatchRepository matchRepository, IEnumerable<IScoringRule> rules) : IMatchService
{
    private readonly IMatchRepository _repo = matchRepository;
    private readonly Dictionary<GameType, IScoringRule> _rules = rules.ToDictionary(r => r.Game);
    public async Task<Result<MatchResponse>> CreateAsync(CreateMatchRequest request, CancellationToken ct = default)
    {
        if(!_rules.TryGetValue(request.Game, out var rule))
            return MatchErrors.NoRuleForGame(request.Game);
        if(request.Results.Count <= 0)
            return MatchErrors.NoResults;
        if(request.Results.Select(r => r.PlayerId).Distinct().Count() != request.Results.Count)
            return MatchErrors.PlayersNotUnique;

        var match = new Match
        {
          MatchId = Guid.NewGuid(),
          Game = request.Game,
          PlayedAt = request.PlayedAt  
        };

        foreach(var input in request.Results)
        {
            var result = new MatchResult
            {
                MatchResultId = Guid.NewGuid(),
                MatchId = match.MatchId,
                UserId = input.PlayerId,
                IsWinner = input.IsWinner,
                TotalScore = input.TotalScore,
                ScoreComponents = [.. (input.Components ?? [])
                    .Select(c => new ScoreComponent
                    {
                        ComponetId = Guid.NewGuid(),
                        Order = c.Order,
                        Label = c.Label,
                        Points = c.Points
                    })]
            };

            var validationResult = rule!.Validate(result);
            if(validationResult.IsFailure)
                return validationResult.Error;

            result.TotalScore = rule.CalculateTotal(result);

            match.Results.Add(result);
        }

        await _repo.AddAsync(match, ct);
        return match.ToResponse();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if(!await _repo.DeleteAsync(id, ct))
            return MatchErrors.NotFound;
        
        return Result.Success();
    }

    public async Task<Result<MatchResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var match = await _repo.GetByMatchIdAsync(id, ct);
        if(match is null)
            return MatchErrors.NotFound;
        
        return match.ToResponse();
    }

    public async Task<Result<IReadOnlyList<MatchResponse>>> GetForUserByGameAsync(Guid userId, GameType game, CancellationToken ct = default)
    {
        var matches = await _repo.GetByUserIdForGameAsync(userId, game, ct);
        if(matches.Count == 0)
            return MatchErrors.NotFound;

        return matches.Select(m => m.ToResponse()).ToList();   
    }
}