using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using TendersApi.Application.Interfaces.Repositories;
using TendersApi.Application.Models;
using TendersApi.Application.Options;

namespace TendersApi.Application.Queries.GetTenders;

internal sealed class GetTendersQueryHandler(ITenderRepository _tenderRepository, IOptions<TendersQueryingOptions> queryingOptions,
    IValidator<GetTendersQuery> _validator)
    : IRequestHandler<GetTendersQuery, PaginatedModel<TenderModel>>
{
    private readonly TendersQueryingOptions _queryingOptions = queryingOptions.Value;

    public async Task<PaginatedModel<TenderModel>> Handle(GetTendersQuery request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var tenders = await _tenderRepository.GetTendersAsync(cancellationToken);

        tenders = ApplyFilters(tenders, request);
        tenders = ApplySorting(tenders, request.Sort);

        var tendersResult = tenders.ToArray();
        var totalCount = tendersResult.Length;
        var totalPages = (int)Math.Ceiling((decimal)totalCount / _queryingOptions.PageSize);

        tendersResult = [.. tendersResult
            .Skip((request.Page - 1) * _queryingOptions.PageSize)
            .Take(_queryingOptions.PageSize)];

        return new PaginatedModel<TenderModel>(request.Page, totalPages, tendersResult);
    }

    private IQueryable<TenderModel> ApplyFilters(IQueryable<TenderModel> queryable, GetTendersQuery query)
    {
        if (query.TenderId.HasValue)
            queryable = queryable.Where(t => t.Id == query.TenderId);

        if (query.SupplierId.HasValue)
            queryable = queryable.Where(t => t.Suppliers.Any(s => s.Id == query.SupplierId));

        if (query.MinDate.HasValue)
            queryable = queryable.Where(t => t.Date >= query.MinDate);

        if (query.MaxDate.HasValue)
            queryable = queryable.Where(t => t.Date <= query.MaxDate);

        if (query.MinAmountInEur.HasValue)
            queryable = queryable.Where(t => t.AmountInEur >= query.MinAmountInEur);

        if (query.MaxAmountInEur.HasValue)
            queryable = queryable.Where(t => t.AmountInEur <= query.MaxAmountInEur);

        return queryable;
    }

    private IQueryable<TenderModel> ApplySorting(IQueryable<TenderModel> queryable, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
            return queryable;

        var parts = sort.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var field = parts[0];
        var direction = parts.Length > 1 ? parts[1] : "asc";

        if (direction is not ("asc" or "desc"))
        {
            throw new ValidationException($"Invalid sort direction: {direction}");
        }

        var sortMap = new Dictionary<string, Expression<Func<TenderModel, object>>>
        {
            [nameof(TenderModel.Date).ToLowerInvariant()] = t => t.Date,
            [nameof(TenderModel.AmountInEur).ToLowerInvariant()] = t => t.AmountInEur
        };

        if (!sortMap.TryGetValue(field, out var keySelector))
        {
            throw new ValidationException($"Invalid sort field: {field}");
        }

        return direction == "asc"
            ? queryable.OrderBy(keySelector)
            : queryable.OrderByDescending(keySelector);
    }
}
