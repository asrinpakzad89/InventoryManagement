namespace Domain;

public class SaleInvoice
    : BaseEntity<int>
{
    public string InvoiceNumber { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public ICollection<SaleInvoiceItem> Items { get; set; } = new List<SaleInvoiceItem>();
}
