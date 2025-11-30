namespace Application.Common.ViewModels.SaleInvoices;


public class SaleInvoiceListDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null;
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
    public List<SaleInvoiceItemDto> Items { get; set; } = new();
}
public class SaleInvoiceItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal? MaxQuantity { get; set; }
    public decimal SalePrice { get; set; }
}

public class SaleFormDataDto
{
    public List<ProductDto> Products { get; set; } = new();
    public List<CustomerDto> Customers { get; set; } = new();
}
