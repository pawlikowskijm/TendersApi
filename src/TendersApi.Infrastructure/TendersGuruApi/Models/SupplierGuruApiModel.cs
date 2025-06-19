using System.Text.Json.Serialization;
using TendersApi.Application.Models;

namespace TendersApi.Infrastructure.TendersGuruApi.Models;

public record SupplierGuruApiModel
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    public SupplierModel ToSupplierModel()
    {
        return new SupplierModel(Id, Name);
    }
}
