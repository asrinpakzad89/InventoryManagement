namespace Application.Common.ViewModels.PurchaseInvoice;

public class PurchaseInvoiceListDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
    public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
}
public class PurchaseInvoiceItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
}

public class PurchaseFormDataDto
{
    public List<ProductDto> Products { get; set; } = new();
    public List<SupplierDto> Suppliers { get; set; } = new();
}