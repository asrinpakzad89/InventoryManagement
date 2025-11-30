namespace Application.Features.PurchaseInvoices.Commands.Update;

public class UpdatePurchaseInvoiceCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int SupplierId { get; set; }
    public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
}
