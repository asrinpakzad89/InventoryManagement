namespace Application.Features.SaleInvoices.Commands.Update;

public class UpdateSaleInvoicesCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int CustomerId { get; set; }
    public List<SaleInvoiceItemDto> Items { get; set; } = new();
}
