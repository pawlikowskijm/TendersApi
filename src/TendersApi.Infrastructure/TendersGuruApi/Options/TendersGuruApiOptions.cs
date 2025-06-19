namespace TendersApi.Infrastructure.TendersWebApi.Options;

public record TendersGuruApiOptions
{
    public required string Host { get; init; }
    public required string TendersResource { get; init; }
    public required int TendersDataCacheInMinutes { get; init; } = 60;
}
