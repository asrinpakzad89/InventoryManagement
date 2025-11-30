using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseInvoices.Queries.GetPurchaseInvoices;

public class GetPurchaseInvoiceQueryHandler
    : IRequestHandler<GetPurchaseInvoiceQuery, List<PurchaseInvoiceListDto>>
{
    private readonly IEFRepository<PurchaseInvoice> _repository;
    private readonly IMapper _mapper;
    public GetPurchaseInvoiceQueryHandler(IEFRepository<PurchaseInvoice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    async Task<List<PurchaseInvoiceListDto>> IRequestHandler<GetPurchaseInvoiceQuery, List<PurchaseInvoiceListDto>>.Handle(GetPurchaseInvoiceQuery request, CancellationToken cancellationToken)
    {

        var query = _repository.Query()
                    .Include(x => x.Supplier)
                    .Include(x => x.Items)
                    .ThenInclude(i => i.Product)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.ProductName))
            query = query.Where(x => x.Items
                .Any(i => i.Product.Name.Contains(request.ProductName)));

        if (!string.IsNullOrWhiteSpace(request.InvoiceNumber))
            query = query.Where(x => x.InvoiceNumber.Contains(request.InvoiceNumber));

        if (request.SupplierId.HasValue)
            query = query.Where(x => x.SupplierId == request.SupplierId);

        if (request.FromDate.HasValue)
            query = query.Where(x => x.Date >= request.FromDate);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.Date <= request.ToDate);

        var invoices = await query
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);

        // محاسبه جمع کل هر فاکتور
        var result = invoices.Select(x => new PurchaseInvoiceListDto
        {
            Id = x.Id,
            InvoiceNumber = x.InvoiceNumber,
            SupplierName = x.Supplier.Name,
            Date = x.Date,
            TotalPrice = x.Items.Sum(i => i.Quantity * i.PurchasePrice),
            Items = x.Items.Select(i => new PurchaseInvoiceItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                PurchasePrice = i.PurchasePrice,
                SalePrice = i.SalePrice
            }).ToList()
        }).ToList();

        return result;
    }
}
