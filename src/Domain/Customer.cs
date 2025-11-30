namespace Domain;

public class Customer
    : BaseEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Contact { get; set; }

    public ICollection<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();
}