namespace Backend.Domain.Entities;

public class ScoreComponent
{
    public Guid ComponetId { get; set;}
    public Guid MatchResultId { get; set;}
    public int Order { get; set;}
    public string? Label { get; set;}
    public int Points { get; set;}
}
