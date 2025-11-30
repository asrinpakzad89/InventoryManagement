using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Features.PurchaseInvoices.Queries.GetPurchaseInvoice;

public class GetPurchaseInvoiceByIdQueryHandler : IRequestHandler<GetPurchaseInvoiceByIdQuery, PurchaseInvoiceListDto>
{
    private readonly IEFRepository<PurchaseInvoice> _repository;
    private readonly IMapper _mapper;
    private GetPurchaseInvoiceByIdQuery _query;
    public GetPurchaseInvoiceByIdQueryHandler(IEFRepository<PurchaseInvoice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _query = new();
    }
    async Task<PurchaseInvoiceListDto> IRequestHandler<GetPurchaseInvoiceByIdQuery, PurchaseInvoiceListDto>.Handle(GetPurchaseInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        _query = request;

        var query = _repository.Query()
                            .Include(x => x.Supplier)
                            .Include(x => x.Items)
                            .ThenInclude(i => i.Product)
                            .AsQueryable();

        var invoices = await query
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);

        var result = invoices.Select(x => new PurchaseInvoiceListDto
        {
            Id = x.Id,
            InvoiceNumber = x.InvoiceNumber,
            SupplierId = x.Supplier.Id,
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
        }).FirstOrDefault(x => x.Id == _query.Id);

        return result;
    }
}
