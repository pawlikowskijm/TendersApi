using FluentValidation;
using Microsoft.Extensions.Options;
using TendersApi.Application.Options;

namespace TendersApi.Application.Queries.GetTenders;

public sealed class GetTendersQueryValidator : AbstractValidator<GetTendersQuery>
{
    public GetTendersQueryValidator(IOptions<TendersQueryingOptions> _queryingOptions)
    {
        var queryingOptions = _queryingOptions.Value;

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(queryingOptions.MinPage)
            .WithMessage($"{nameof(GetTendersQuery.Page)} must be greater than or equal to {queryingOptions.MinPage}.")
            .LessThanOrEqualTo(queryingOptions.MaxPage)
            .WithMessage($"{nameof(GetTendersQuery.Page)} must be lesser than or equal to {queryingOptions.MaxPage}.");
    }
}
