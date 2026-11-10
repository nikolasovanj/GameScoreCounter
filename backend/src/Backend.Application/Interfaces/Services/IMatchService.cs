using Backend.Application.Common;
using Backend.Application.DTOs;
using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Domain.Enums;
using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Services;

public interface IMatchService
{
    Task<Result<MatchResponse>> CreateAsync(CreateMatchRequest request, CancellationToken ct = default);
    Task<Result<MatchResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<PagedResult<MatchResponse>>> GetForUserByGameAsync(Guid userId, GameType game, PageRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<Result<PagedResult<PlayerMatchResultDTO>>> GetPlayerResultsAsync(Guid userId, GameType game, PageRequest request, CancellationToken ct = default);
}