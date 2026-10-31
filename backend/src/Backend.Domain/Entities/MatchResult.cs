namespace Backend.Domain.Entities;

public class MatchResult
{
    public Guid MatchResultId { get; set;}
    public Guid MatchId { get; set;}
    public Guid UserId { get; set;}
    public bool IsWinner { get; set;}
    public int? TotalScore { get; set;}
    public List<ScoreComponent> ScoreComponents { get; set;} = [];
}
