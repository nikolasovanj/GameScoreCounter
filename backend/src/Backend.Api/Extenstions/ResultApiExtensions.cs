using Backend.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Extenstions;

public static class ResultApiExtensions
{
public static ActionResult ToApiResult<T>(this Result<T> result)
{
    return result.IsSuccess ? new OkObjectResult(result.Value) : HandelFailure(result);
}

public static ActionResult ToApiResult(this Result result)
{
    return result.IsSuccess ? new OkObjectResult(new { message = "Operation successful."}) : HandelFailure(result);
}

private static ObjectResult HandelFailure(Result result) => 
    result.Error switch
    {
        NotFoundError => new NotFoundObjectResult(result.Error),
        ValidationError => new BadRequestObjectResult(result.Error),
        ConflictError => new ConflictObjectResult(result.Error),
        UnauthorizedError => new UnauthorizedObjectResult(result.Error),
        _ => new BadRequestObjectResult(result.Error)
    };
}