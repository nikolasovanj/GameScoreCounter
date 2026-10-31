using Backend.Domain.Enums;

namespace Backend.Domain;

public class Match
{
    public int MatchId { get; set;}
    public GameType Game { get; set;}
    public DateTime PlayedAt { get; set;}
    public List<MatchResult> Results { get; set;} = [];
}
