namespace TendersApi.Application.Models;

public record SupplierModel
{
    public int Id { get; }
    public string? Name { get; }

    public SupplierModel(int id, string? name)
    {
        Id = id;
        Name = name;
    }
}
