using System.Text.Json.Serialization;

namespace TendersApi.Infrastructure.TendersGuruApi.Models;

public record SupplierGuruApiModel
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
