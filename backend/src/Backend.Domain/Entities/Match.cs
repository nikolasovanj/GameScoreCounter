using Backend.Domain.Enums;

namespace Backend.Domain.Entities;

public class Match
{
    public Guid MatchId { get; set;}
    public GameType Game { get; set;}
    public DateTime PlayedAt { get; set;}
    public List<MatchResult> Results { get; set;} = [];
}
