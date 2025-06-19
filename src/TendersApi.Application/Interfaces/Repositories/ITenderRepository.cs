using TendersApi.Application.Models;

namespace TendersApi.Application.Interfaces.Repositories;

public interface ITenderRepository
{
    Task<IQueryable<TenderModel>> GetTendersAsync(CancellationToken cancellationToken = default);
    Task<bool> FetchTendersDataAsync(CancellationToken cancellationToken = default);
}
