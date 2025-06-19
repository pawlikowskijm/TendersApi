using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using TendersApi.Application.Interfaces.Repositories;
using TendersApi.Infrastructure.DistributedCache;
using TendersApi.Infrastructure.TendersWebApi.Options;

namespace TendersApi.WebApi.HostedServices;

public class DataInitializationService(IOptions<TendersGuruApiOptions> _apiOptions, ITenderRepository _tenderRepository, IDistributedCache _distributedCache)
    : IHostedService
{
    private const string FetchTendersDataCacheKey = "fetch_tenders_data";
    private TendersGuruApiOptions _apiOptions = _apiOptions.Value;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                var isDataRefreshed = await _distributedCache.GetDataAsync<bool>(FetchTendersDataCacheKey, cancellationToken);

                if (!isDataRefreshed)
                {
                    var isFetchSucceeded = await _tenderRepository.FetchTendersDataAsync(CancellationToken.None);
                    if (isFetchSucceeded)
                    {
                        await _distributedCache.SetDataAsync(FetchTendersDataCacheKey, true, _apiOptions.TendersDataCacheInMinutes, cancellationToken);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(10), CancellationToken.None);
            }
        }, CancellationToken.None);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
