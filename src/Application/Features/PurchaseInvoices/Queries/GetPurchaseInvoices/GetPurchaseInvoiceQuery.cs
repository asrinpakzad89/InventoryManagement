namespace Application.Features.PurchaseInvoices.Queries.GetPurchaseInvoices;

public class GetPurchaseInvoiceQuery : IRequest<List<PurchaseInvoiceListDto>> {

    public string? ProductName { get; set; }
    public string? InvoiceNumber { get; set; }
    public int? SupplierId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
