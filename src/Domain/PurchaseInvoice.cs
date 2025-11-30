namespace Domain;

public class PurchaseInvoice
    : BaseEntity<int>
{
    public string InvoiceNumber { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
}
