using System.Text.Json.Serialization;
using TendersApi.Infrastructure.TendersWebApi.Models;

namespace TendersApi.Infrastructure.TendersGuruApi.Models;

public record ListTendersApiModel
{
    [JsonPropertyName("page_number")]
    public int Page { get; init; }
    [JsonPropertyName("page_count")]
    public int TotalPages { get; init; }
    [JsonPropertyName("data")]
    public IEnumerable<TenderGuruApiModel> Tenders { get; init; } = [];
}
