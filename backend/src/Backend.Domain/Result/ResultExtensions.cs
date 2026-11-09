namespace Backend.Domain.Result;

public static class ResultExtensions
{
    public static Result<T> OnError<T>(this T? value, Error error) where T : class => value is null ? error : value;
}