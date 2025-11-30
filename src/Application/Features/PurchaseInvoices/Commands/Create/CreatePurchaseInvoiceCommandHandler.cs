namespace Application.Features.PurchaseInvoices.Commands.Create;

public class CreatePurchaseInvoiceCommandHandler
    : IRequestHandler<CreatePurchaseInvoiceCommand, int>
{
    private readonly IEFRepository<Product> _productRepository;
    private readonly IEFRepository<PurchaseInvoice> _purchaseInvoiceRepository;
    private readonly IEFRepository<StockTransaction> _stockTransactionsRepository;
    private readonly IMapper _mapper;

    public CreatePurchaseInvoiceCommandHandler(IMapper mapper, IEFRepository<Product> productRepository, IEFRepository<PurchaseInvoice> purchaseInvoiceRepository, IEFRepository<StockTransaction> stockTransactionsRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _purchaseInvoiceRepository = purchaseInvoiceRepository;
        _stockTransactionsRepository = stockTransactionsRepository;
    }

    public async Task<int> Handle(CreatePurchaseInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = new PurchaseInvoice
        {
            InvoiceNumber = request.InvoiceNumber,
            SupplierId = request.SupplierId,
            Date = DateTime.UtcNow,
        };

        invoice = await _purchaseInvoiceRepository.AddAsync(invoice);

        foreach (var item in request.Items)
        {
            invoice.Items.Add(new PurchaseInvoiceItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PurchasePrice = item.PurchasePrice,
                SalePrice = item.SalePrice
            });

            // افزایش موجودی کالا
            Product product = await _productRepository.GetAsync(item.ProductId, cancellationToken);

            // ثبت تراکنش انبار
            await _stockTransactionsRepository.AddAsync(new StockTransaction
            {
                ProductId = item.ProductId,
                QuantityChange = item.Quantity,
                SourceType = SourceTypeEnum.PurchaseInvoice,
                SourceId = invoice.Id,
                Date = DateTime.UtcNow,
                Note = $" فاکتور خرید {request.InvoiceNumber}",
                OldPurchasePrice = product.PurchasePrice,
                NewPurchasePrice = item.PurchasePrice,
                OldSalePrice = product.SellingPrice,
                NewSalePrice = item.SalePrice
            });

            product.Quantity += item.Quantity;
            product.SupplierId = invoice.SupplierId;
            product.PurchasePrice = item.PurchasePrice;
            product.SellingPrice = item.SalePrice;

            await _productRepository.UpdateAsync(product);

        }
        await _purchaseInvoiceRepository.SaveChangeAsync(cancellationToken);
        await _stockTransactionsRepository.SaveChangeAsync(cancellationToken);
        await _productRepository.SaveChangeAsync(cancellationToken);

        return invoice.Id;
    }
}