namespace Application.Features.SaleInvoices.Commands.Create;

public class CreateSaleInvoicesCommandHandler : IRequestHandler<CreateSaleInvoicesCommand, int>
{
    private readonly IEFRepository<SaleInvoice> _invoiceRepo;
    private readonly IEFRepository<Product> _productRepo;
    private readonly IEFRepository<StockTransaction> _stockRepo;
    private readonly IMapper _mapper;

    public CreateSaleInvoicesCommandHandler(
        IEFRepository<SaleInvoice> invoiceRepo,
        IEFRepository<Product> productRepo,
        IEFRepository<StockTransaction> stockRepo,
        IMapper mapper)
    {
        _invoiceRepo = invoiceRepo;
        _productRepo = productRepo;
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateSaleInvoicesCommand request, CancellationToken cancellationToken)
    {
        if (request.Items == null || !request.Items.Any())
            throw new ArgumentException("هیچ آیتمی برای فاکتور ارسال نشده.");

        var invoice = new SaleInvoice
        {
            InvoiceNumber = request.InvoiceNumber,
            Date = DateTime.UtcNow,
            CustomerId = request.CustomerId
        };

        foreach (var it in request.Items)
        {
            var product = await _productRepo.GetAsync(it.ProductId, cancellationToken);
            if (product == null) throw new KeyNotFoundException($"محصول {it.ProductId} یافت نشد.");

            if (product.Quantity < it.Quantity)
                throw new InvalidOperationException($"موجودی محصول '{product.Name}' کافی نیست.");

            invoice.Items.Add(new SaleInvoiceItem
            {
                ProductId = it.ProductId,
                Quantity = it.Quantity,
                UnitPrice = it.SalePrice
            });

            var oldPurchase = product.PurchasePrice;
            var oldSale = product.SellingPrice;

            product.Quantity -= it.Quantity;

            await _stockRepo.AddAsync(new StockTransaction
            {
                ProductId = product.Id,
                QuantityChange = -it.Quantity,
                OldPurchasePrice = oldPurchase,
                NewPurchasePrice = product.PurchasePrice,
                OldSalePrice = oldSale,
                NewSalePrice = product.SellingPrice,
                SourceType = SourceTypeEnum.SaleInvoice,
                SourceId = invoice.Id,
                Date = DateTime.UtcNow,
                Note = $"فروش از فاکتور {request.InvoiceNumber}"
            }, cancellationToken);

            await _productRepo.UpdateAsync(product, cancellationToken);
        }

        await _invoiceRepo.AddAsync(invoice, cancellationToken);
        await _invoiceRepo.SaveChangeAsync(cancellationToken);
        await _productRepo.SaveChangeAsync(cancellationToken);
        await _stockRepo.SaveChangeAsync(cancellationToken);

        return invoice.Id;
    }
}
