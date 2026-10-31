namespace Backend.Domain;

public static class ScoreComponentErrors
{
    public static Error NotFound => 
        Error.NotFound("ScoreComponent.NotFound", "Score component was not found.");
}
