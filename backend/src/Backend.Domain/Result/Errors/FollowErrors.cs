namespace Backend.Domain.Result.Errors;

public static class FollowErrors
{
    public static Error SelfFollow =>
        Error.Validation("FollowResult.SelfFollow", "You cannot follow yourself.");
}