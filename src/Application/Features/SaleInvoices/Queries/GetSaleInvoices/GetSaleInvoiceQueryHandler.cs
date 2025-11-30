using Microsoft.EntityFrameworkCore;

namespace Application.Features.SaleInvoices.Queries.GetSaleInvoices;

public class GetSaleInvoiceQueryHandler
    : IRequestHandler<GetSaleInvoiceQuery, List<SaleInvoiceListDto>>
{
    private readonly IEFRepository<SaleInvoice> _repository;
    private readonly IMapper _mapper;
    public GetSaleInvoiceQueryHandler(IEFRepository<SaleInvoice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    async Task<List<SaleInvoiceListDto>> IRequestHandler<GetSaleInvoiceQuery, List<SaleInvoiceListDto>>.Handle(GetSaleInvoiceQuery request, CancellationToken cancellationToken)
    {

        var query = _repository.Query()
                    .Include(x => x.Customer)
                    .Include(x => x.Items)
                    .ThenInclude(i => i.Product)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.ProductName))
            query = query.Where(x => x.Items
                .Any(i => i.Product.Name.Contains(request.ProductName)));

        if (!string.IsNullOrWhiteSpace(request.InvoiceNumber))
            query = query.Where(x => x.InvoiceNumber.Contains(request.InvoiceNumber));

        if (request.CustomerId.HasValue)
            query = query.Where(x => x.CustomerId == request.CustomerId);

        if (request.FromDate.HasValue)
            query = query.Where(x => x.Date >= request.FromDate);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.Date <= request.ToDate);

        var invoices = await query
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);

        // محاسبه جمع کل هر فاکتور
        var result = invoices.Select(x => new SaleInvoiceListDto
        {
            Id = x.Id,
            InvoiceNumber = x.InvoiceNumber,
            CustomerName = x.Customer.Name,
            Date = x.Date,
            TotalPrice = x.Items.Sum(i => i.Quantity * i.UnitPrice),
            Items = x.Items.Select(i => new SaleInvoiceItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                SalePrice = i.UnitPrice,
            }).ToList()
        }).ToList();

        return result;
    }
}
