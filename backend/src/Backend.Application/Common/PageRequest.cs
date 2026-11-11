namespace Backend.Application.Common;

public record PageRequest
{
    public const int MaxPageSize = 100;
    public const int DefaultPageSize = 20;

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = DefaultPageSize;

    public int GetSafePage() => Math.Max(Page, 1);
    public int GetSafePageSize() => Math.Clamp(PageSize, 1, MaxPageSize);
    public int GetSkip() => (GetSafePage() -1 ) * GetSafePageSize();
}