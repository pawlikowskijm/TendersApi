using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using TendersApi.Application.Interfaces.Repositories;
using TendersApi.Application.Models;
using TendersApi.Application.Options;
using TendersApi.Infrastructure.DistributedCache;
using TendersApi.Infrastructure.TendersGuruApi.Models;
using TendersApi.Infrastructure.TendersWebApi.Options;

namespace TendersApi.Infrastructure.TendersWebApi.Repositories;

internal class TendersGuruApiService(IOptions<TendersGuruApiOptions> apiOptions, IOptions<TendersQueryingOptions> queryingOptions,
    IDistributedCache _distributedCache, IMapper _mapper, IRestClient _restClient, ILogger<TendersGuruApiService> _logger) : ITenderRepository
{
    private const string TendersCacheKey = "tenders";
    private readonly TendersGuruApiOptions _apiOptions = apiOptions.Value;
    private readonly TendersQueryingOptions _queryingOptions = queryingOptions.Value;

    public async Task<IQueryable<TenderModel>> GetTendersAsync(CancellationToken cancellationToken = default)
    {
        var tenders = (await _distributedCache.GetDataAsync<List<TenderModel>>(TendersCacheKey, cancellationToken))?.AsQueryable();

        if (tenders is null)
        {
            throw new ValidationException("Tenders data is not available yet. Please wait for fetching the data.");
        }

        return tenders;
    }

    public async Task<bool> FetchTendersDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var semaphore = new SemaphoreSlim(10);

            var partialTasks = Enumerable.Range(_queryingOptions.MinPage, _queryingOptions.MaxPage).Select(async page =>
            {
                await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    var partialRequest = new RestRequest(_apiOptions.TendersResource, Method.Get);
                    partialRequest.AddQueryParameter("page", page);
                    return (await _restClient.GetAsync<ListTendersApiModel>(partialRequest, cancellationToken).ConfigureAwait(false))!;
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var allPartialResponses = await Task.WhenAll(partialTasks);
            var allTenders = allPartialResponses.SelectMany(r => r.Tenders).Select(_mapper.Map<TenderModel>).ToList();

            await _distributedCache.SetDataAsync(TendersCacheKey, allTenders, _apiOptions.TendersDataCacheInMinutes * 2, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while fetching tenders from {nameof(TendersGuruApiService)}.");

            return false;
        }
    }
}
