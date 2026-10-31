namespace Backend.Domain;

public class ScoreComponent
{
    public int Id { get; set;}
    public int MatchResultId { get; set;}
    public string Label { get; set;} = string.Empty;
    public int Points { get; set;}
}
