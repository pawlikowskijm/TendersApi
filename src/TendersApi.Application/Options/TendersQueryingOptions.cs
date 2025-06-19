namespace TendersApi.Application.Options;

public record TendersQueryingOptions
{
    public required int MinPage { get; init; }
    public required int MaxPage { get; init; }
    public required int PageSize { get; init; } = 100;
}
