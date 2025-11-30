namespace Application.Features.SaleInvoices.Commands.Create;

public class CreateSaleInvoicesCommand: IRequest<int>
{
    public string InvoiceNumber { get; set; }
    public int CustomerId { get; set; }
    public List<SaleInvoiceItemDto> Items { get; set; } = new();

}
