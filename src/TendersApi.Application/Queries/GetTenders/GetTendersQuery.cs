using MediatR;
using TendersApi.Application.Models;

namespace TendersApi.Application.Queries.GetTenders
{
    public record GetTendersQuery(int? TenderId, int? SupplierId, decimal? MinAmountInEur, decimal? MaxAmountInEur,
        DateOnly? MinDate, DateOnly? MaxDate, string? Sort, int Page = 1) : IRequest<PaginatedModel<TenderModel>>;
}
