namespace TendersApi.Application.Models;

public record TenderModel
{
    public int Id { get; }
    public DateOnly Date { get; }
    public string Title { get; }
    public string Description { get; }
    public decimal AmountInEur { get; }
    public IEnumerable<SupplierModel> Suppliers { get; }

    public TenderModel(int id, DateOnly date, string title, string description, decimal amountInEur, IEnumerable<SupplierModel> suppliers)
    {
        Id = id;
        Date = date;
        Title = title;
        Description = description;
        AmountInEur = amountInEur;
        Suppliers = suppliers;
    }
}
