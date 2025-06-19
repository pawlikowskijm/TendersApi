using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace TendersApi.Infrastructure.DistributedCache;

public static class Extensions
{
    public static async Task<T?> GetDataAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var bytes = await cache.GetAsync(key, cancellationToken);

        if (bytes is null)
            return default;

        var json = Encoding.UTF8.GetString(bytes);

        return JsonSerializer.Deserialize<T>(json);
    }

    public static async Task SetDataAsync<T>(this IDistributedCache cache, string key, T data, int? expirationMinutes = default,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(data);
        var bytes = Encoding.UTF8.GetBytes(json);

        if (expirationMinutes.HasValue)
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes.Value) };
            await cache.SetAsync(key, bytes, options, cancellationToken);
        }

        await cache.SetAsync(key, bytes, cancellationToken);
    }
}
