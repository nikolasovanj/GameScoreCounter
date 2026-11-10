using Backend.Api.Contracts;
using Backend.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Extenstions;

public static class ResultApiExtensions
{
    public static ActionResult ToApiResult<T>(this Result<T> result)
    {
        return result.IsSuccess ? new OkObjectResult(ApiResponseFactory.ValueResponse(result.Value)) : HandelFailure(result);
    }

    public static ActionResult ToApiResult(this Result result)
    {
        return result.IsSuccess ? new OkObjectResult(ApiResponseFactory.Response()) : HandelFailure(result);
    }

    private static ObjectResult HandelFailure(Result result) =>
        result.Error switch
        {
            NotFoundError => new NotFoundObjectResult(ApiResponseFactory.ErrorResponse(result.Error)),
            ValidationError => new BadRequestObjectResult(ApiResponseFactory.ErrorResponse(result.Error)),
            ConflictError => new ConflictObjectResult(ApiResponseFactory.ErrorResponse(result.Error)),
            UnauthorizedError => new UnauthorizedObjectResult(ApiResponseFactory.ErrorResponse(result.Error)),
            _ => new BadRequestObjectResult(ApiResponseFactory.ErrorResponse(result.Error))
        };
}