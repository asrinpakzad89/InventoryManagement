using Microsoft.EntityFrameworkCore;

namespace Application.Features.SaleInvoices.Commands.Update;

public class UpdateSaleInvoicesCommandHandler : IRequestHandler<UpdateSaleInvoicesCommand, bool>
{
    private readonly IEFRepository<SaleInvoice> _invoiceRepository;
    private readonly IEFRepository<Product> _productRepository;
    private readonly IEFRepository<StockTransaction> _stockTransactionRepository;

    public UpdateSaleInvoicesCommandHandler(
        IEFRepository<SaleInvoice> invoiceRepo,
        IEFRepository<Product> productRepo,
        IEFRepository<StockTransaction> stockTxnRepo)
    {
        _invoiceRepository = invoiceRepo;
        _productRepository = productRepo;
        _stockTransactionRepository = stockTxnRepo;
    }

    public async Task<bool> Handle(UpdateSaleInvoicesCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.Query()
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice == null)
            throw new KeyNotFoundException("فاکتور یافت نشد.");

        var newItemsDict = request.Items.ToDictionary(i => i.ProductId);
        var oldItemsDict = invoice.Items.ToDictionary(i => i.ProductId);

        var removedIds = oldItemsDict.Keys.Except(newItemsDict.Keys).ToList();

        foreach (var pid in removedIds)
        {
            var old = oldItemsDict[pid];
            var product = await _productRepository.GetAsync(pid, cancellationToken);
            if (product == null) continue;

            product.Quantity += old.Quantity; 

            await _stockTransactionRepository.AddAsync(new StockTransaction
            {
                ProductId = product.Id,
                QuantityChange = +old.Quantity,
                OldPurchasePrice = product.PurchasePrice,
                NewPurchasePrice = product.PurchasePrice,
                OldSalePrice = product.SellingPrice,
                NewSalePrice = product.SellingPrice,
                SourceType = SourceTypeEnum.UpdateSaleInvoice,
                SourceId = invoice.Id,
                Date = DateTime.UtcNow,
                Note = "حذف آیتم از فاکتور فروش و برگشت به انبار"
            }, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        var addedIds = newItemsDict.Keys.Except(oldItemsDict.Keys).ToList();

        foreach (var pid in addedIds)
        {
            var newItem = newItemsDict[pid];
            var product = await _productRepository.GetAsync(pid, cancellationToken);
            if (product == null) continue;

            product.Quantity -= newItem.Quantity; 

            var oldSale = product.SellingPrice;


            product.SellingPrice = newItem.SalePrice;

            await _stockTransactionRepository.AddAsync(new StockTransaction
            {
                ProductId = product.Id,
                QuantityChange = -newItem.Quantity,
                OldPurchasePrice = product.PurchasePrice,
                NewPurchasePrice = product.PurchasePrice,
                OldSalePrice = oldSale,
                NewSalePrice = product.SellingPrice,
                SourceType = SourceTypeEnum.UpdateSaleInvoice,
                SourceId = invoice.Id,
                Date = DateTime.UtcNow,
                Note = "اضافه شدن آیتم جدید در ویرایش فاکتور فروش"
            }, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        var commonIds = oldItemsDict.Keys.Intersect(newItemsDict.Keys).ToList();

        foreach (var pid in commonIds)
        {
            var oldItem = oldItemsDict[pid];
            var newItem = newItemsDict[pid];

            var product = await _productRepository.GetAsync(pid, cancellationToken);
            if (product == null) continue;

            decimal qtyDelta = newItem.Quantity - oldItem.Quantity;
            bool salePriceChanged = newItem.SalePrice != oldItem.UnitPrice;

            if (qtyDelta != 0)
                product.Quantity -= qtyDelta;

            var oldSalePrice = product.SellingPrice;
            if (salePriceChanged)
                product.SellingPrice = newItem.SalePrice;

            if (qtyDelta != 0 || salePriceChanged)
            {
                await _stockTransactionRepository.AddAsync(new StockTransaction
                {
                    ProductId = pid,
                    QuantityChange = -qtyDelta,
                    OldPurchasePrice = product.PurchasePrice,
                    NewPurchasePrice = product.PurchasePrice,
                    OldSalePrice = oldSalePrice,
                    NewSalePrice = product.SellingPrice,
                    SourceType = SourceTypeEnum.UpdateSaleInvoice,
                    SourceId = invoice.Id,
                    Date = DateTime.UtcNow,
                    Note = "ویرایش آیتم فاکتور فروش"
                }, cancellationToken);

                await _productRepository.UpdateAsync(product, cancellationToken);
            }
        }

        invoice.InvoiceNumber = request.InvoiceNumber;
        invoice.CustomerId = request.CustomerId;

        // آیتم‌ها کاملاً جایگزین می‌شوند
        invoice.Items.Clear();
        foreach (var item in request.Items)
        {
            invoice.Items.Add(new SaleInvoiceItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.SalePrice
            });
        }

        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

        await _invoiceRepository.SaveChangeAsync(cancellationToken);
        await _productRepository.SaveChangeAsync(cancellationToken);
        await _stockTransactionRepository.SaveChangeAsync(cancellationToken);

        return true;
    }
}