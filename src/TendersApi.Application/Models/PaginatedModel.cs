namespace TendersApi.Application.Models;

public record PaginatedModel<T>
{
    public int Page { get; }
    public int TotalPages { get; }
    public IEnumerable<T> Items { get; }

    public PaginatedModel(int page, int totalPages, IEnumerable<T> items)
    {
        Page = page;
        TotalPages = totalPages;
        Items = items;
    }
}
