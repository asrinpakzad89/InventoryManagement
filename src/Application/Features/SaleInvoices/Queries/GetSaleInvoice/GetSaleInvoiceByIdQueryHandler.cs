using Microsoft.EntityFrameworkCore;

namespace Application.Features.SaleInvoices.Queries.GetSaleInvoice;

public class GetSaleInvoiceByIdQueryHandler : IRequestHandler<GetSaleInvoiceByIdQuery, SaleInvoiceListDto>
{
    private readonly IEFRepository<SaleInvoice> _repository;
    private readonly IMapper _mapper;
    private GetSaleInvoiceByIdQuery _query;
    public GetSaleInvoiceByIdQueryHandler(IEFRepository<SaleInvoice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _query = new();
    }
    async Task<SaleInvoiceListDto> IRequestHandler<GetSaleInvoiceByIdQuery, SaleInvoiceListDto>.Handle(GetSaleInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        _query = request;

        var query = _repository.Query()
                            .Include(x => x.Customer)
                            .Include(x => x.Items)
                            .ThenInclude(i => i.Product)
                            .AsQueryable();

        var invoices = await query
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);

        var result = invoices.Select(x => new SaleInvoiceListDto
        {
            Id = x.Id,
            InvoiceNumber = x.InvoiceNumber,
            CustomerId = x.Customer.Id,
            CustomerName = x.Customer.Name,
            Date = x.Date,
            TotalPrice = x.Items.Sum(i => i.Quantity * i.UnitPrice),
            Items = x.Items.Select(i => new SaleInvoiceItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                MaxQuantity = i.Product.Quantity,
                SalePrice = i.UnitPrice,
            }).ToList()
        }).FirstOrDefault(x => x.Id == _query.Id);

        return result;
    }
}
