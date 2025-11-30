namespace Application.Features.SaleInvoices.Queries.GetSaleInvoice;

public class GetSaleInvoiceByIdQuery : IRequest<SaleInvoiceListDto>
{
    public int Id { get; set; }
}