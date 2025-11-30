namespace Application.Features.PurchaseInvoices.Queries.GetPurchaseInvoice;

public class GetPurchaseInvoiceByIdQuery : IRequest<PurchaseInvoiceListDto>
{
    public int Id { get; set; }
}