using System.Text.Json.Serialization;
using TendersApi.Infrastructure.TendersGuruApi.Models;

namespace TendersApi.Infrastructure.TendersWebApi.Models;

public record TenderGuruApiModel
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("date")]
    public DateOnly Date { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;
    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;
    [JsonPropertyName("awarded_value_eur")]
    public decimal AwardedValueEur { get; init; }
    [JsonPropertyName("awarded")]
    public IEnumerable<AwardedGuruApiModel> Awarded { get; init; } = [];
}
