namespace Backend.Domain;

public class MatchResult
{
    public int MatchResultId { get; set;}
    public int MatchId { get; set;}
    public int UserId { get; set;}
    public bool IsWinner { get; set;}
    public int? TotalScore { get; set;}
    public List<ScoreComponent> ScoreComponents { get; set;} = [];
}
