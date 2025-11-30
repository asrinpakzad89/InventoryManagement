namespace Application.Features.PurchaseInvoices.Commands.Create;

public class CreatePurchaseInvoiceCommand : IRequest<int>
{
    public string InvoiceNumber { get; set; }
    public int SupplierId { get; set; }
    public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
}
