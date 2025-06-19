using System.Text.Json.Serialization;

namespace TendersApi.Infrastructure.TendersGuruApi.Models;

public record AwardedGuruApiModel
{
    [JsonPropertyName("suppliers")]
    public IEnumerable<SupplierGuruApiModel> Suppliers { get; init; } = [];
}
