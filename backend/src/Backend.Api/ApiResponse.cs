using Backend.Api.Extenstions;
using Backend.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api;

public record ApiResponse(bool Success, string? Message);
public record ApiResponse<T>(T Data) : ApiResponse(true, null);

public static class ApiResponseFactory
{
    public static ApiResponse Response() => new(true, "Operation successful");
    public static ApiResponse ErrorResponse(Error error) => new(false, error.Description);
    public static ApiResponse<T> ValueResponse<T>(T Data) => new(Data);
}