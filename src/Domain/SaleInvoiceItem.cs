namespace Domain;

public class SaleInvoiceItem
    : BaseEntity<int>
{
    public int SaleInvoiceId { get; set; }
    public SaleInvoice SaleInvoice { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
