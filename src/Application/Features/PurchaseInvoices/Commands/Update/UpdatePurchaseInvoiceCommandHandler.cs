using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseInvoices.Commands.Update;

public class UpdatePurchaseInvoiceCommandHandler : IRequestHandler<UpdatePurchaseInvoiceCommand, bool>
{
    private readonly IEFRepository<PurchaseInvoice> _invoiceRepository;
    private readonly IEFRepository<Product> _productRepository;
    private readonly IEFRepository<StockTransaction> _stockTransactionRepository;

    public UpdatePurchaseInvoiceCommandHandler(
        IEFRepository<PurchaseInvoice> invoiceRepo,
        IEFRepository<Product> productRepo,
        IEFRepository<StockTransaction> stockTxnRepo)
    {
        _invoiceRepository = invoiceRepo;
        _productRepository = productRepo;
        _stockTransactionRepository = stockTxnRepo;
    }

    public async Task<bool> Handle(UpdatePurchaseInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.Query()
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice == null)
            throw new KeyNotFoundException("فاکتور خرید یافت نشد.");

        var newItemsDict = request.Items.ToDictionary(i => i.ProductId);

        var oldItemsDict = invoice.Items.ToDictionary(i => i.ProductId);

        var removedProductIds = oldItemsDict.Keys.Except(newItemsDict.Keys).ToList();

        foreach (var pid in removedProductIds)
        {
            var oldItem = oldItemsDict[pid];
            var product = await _productRepository.GetAsync(pid, cancellationToken);
            if (product == null) continue;

            var lastTxn = await _stockTransactionRepository.Query()
                .Where(t => t.ProductId == pid &&
                            (t.SourceType == SourceTypeEnum.UpdatePurchaseInvoice || t.SourceType==SourceTypeEnum.PurchaseInvoice) &&
                            t.SourceId == invoice.Id)
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (lastTxn != null)
            {
                product.PurchasePrice = (decimal) lastTxn.OldPurchasePrice;
                product.SellingPrice = (decimal) lastTxn.OldSalePrice;
            }

            var quantityChange = -oldItem.Quantity;
            product.Quantity += quantityChange;

            await _stockTransactionRepository.AddAsync(new StockTransaction
            {
                ProductId = product.Id,
                QuantityChange = quantityChange,
                OldPurchasePrice = lastTxn?.NewPurchasePrice ?? product.PurchasePrice,
                NewPurchasePrice = product.PurchasePrice,      
                OldSalePrice = lastTxn?.NewSalePrice ?? product.SellingPrice,
                NewSalePrice = product.SellingPrice,
                SourceType = SourceTypeEnum.UpdatePurchaseInvoice,
                SourceId = invoice.Id,
                Date = DateTime.UtcNow,
                Note = "حذف آیتم از فاکتور خرید و برگشت قیمت به مقدار قبلی"
            }, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken);
        }


        var addedProductIds = newItemsDict.Keys.Except(oldItemsDict.Keys).ToList();
        foreach (var pid in addedProductIds)
        {
            var newItem = newItemsDict[pid];
            var product = await _productRepository.GetAsync(pid, cancellationToken);
            if (product == null) continue;

            var quantityChange = newItem.Quantity;
            product.Quantity += quantityChange;

            var oldPurchase = product.PurchasePrice;
            var oldSale = product.SellingPrice;

            product.PurchasePrice = newItem.PurchasePrice;
            product.SellingPrice = newItem.SalePrice;


            await _stockTransactionRepository.AddAsync(new StockTransaction
            {
                ProductId = product.Id,
                QuantityChange = quantityChange,
                OldPurchasePrice = oldPurchase,
                NewPurchasePrice = product.PurchasePrice,
                OldSalePrice = oldSale,
                NewSalePrice = product.SellingPrice,
                SourceType = SourceTypeEnum.UpdatePurchaseInvoice,
                SourceId = invoice.Id,
                Date = DateTime.UtcNow,
                Note = "اضافه شدن آیتم به فاکتور خرید (ویرایش)"
            }, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        var commonProductIds = oldItemsDict.Keys.Intersect(newItemsDict.Keys).ToList();
        foreach (var pid in commonProductIds)
        {
            var oldItem = oldItemsDict[pid];
            var newItem = newItemsDict[pid];
            var product = await _productRepository.GetAsync(pid, cancellationToken);
            if (product == null) continue;

            var qtyDelta = newItem.Quantity - oldItem.Quantity;

            var purchasePriceChanged = newItem.PurchasePrice != oldItem.PurchasePrice;
            var salePriceChanged = newItem.SalePrice != oldItem.SalePrice;

            if (qtyDelta != 0 || purchasePriceChanged || salePriceChanged)
            {
                if (qtyDelta != 0)
                {
                    product.Quantity += qtyDelta;
                }

                var oldPurchase = product.PurchasePrice;
                var oldSale = product.SellingPrice;

                if (purchasePriceChanged) product.PurchasePrice = newItem.PurchasePrice;
                if (salePriceChanged) product.SellingPrice = newItem.SalePrice;

                await _stockTransactionRepository.AddAsync(new StockTransaction
                {
                    ProductId = product.Id,
                    QuantityChange = qtyDelta,
                    OldPurchasePrice = oldPurchase,
                    NewPurchasePrice = product.PurchasePrice,
                    OldSalePrice = oldSale,
                    NewSalePrice = product.SellingPrice,
                    SourceType = SourceTypeEnum.UpdatePurchaseInvoice,
                    SourceId = invoice.Id,
                    Date = DateTime.UtcNow,
                    Note = "بروزرسانی آیتم فاکتور خرید (ویرایش)"
                }, cancellationToken);

                await _productRepository.UpdateAsync(product, cancellationToken);
            }
        }

        invoice.InvoiceNumber = request.InvoiceNumber;
        invoice.SupplierId = request.SupplierId;


        invoice.Items.Clear();
        foreach (var newItem in request.Items)
        {
            invoice.Items.Add(new PurchaseInvoiceItem
            {
                ProductId = newItem.ProductId,
                Quantity = newItem.Quantity,
                PurchasePrice = newItem.PurchasePrice,
                SalePrice = newItem.SalePrice
            });
        }

        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

        await _productRepository.SaveChangeAsync(cancellationToken);
        await _stockTransactionRepository.SaveChangeAsync(cancellationToken);
        await _invoiceRepository.SaveChangeAsync(cancellationToken);

        return true;
    }
}
