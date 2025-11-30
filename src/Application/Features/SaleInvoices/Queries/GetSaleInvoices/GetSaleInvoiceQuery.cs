namespace Application.Features.SaleInvoices.Queries.GetSaleInvoices;

public class GetSaleInvoiceQuery : IRequest<List<SaleInvoiceListDto>> {

    public string? ProductName { get; set; }
    public string? InvoiceNumber { get; set; }
    public int? CustomerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
