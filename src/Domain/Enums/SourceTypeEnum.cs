namespace Domain.Enums;

public enum SourceTypeEnum
{
    [Display(Name = "فاکتور خرید")]
    PurchaseInvoice = 1,

    [Display(Name = "فاکتور فروش")]
    SaleInvoice = 2,

    [Display(Name = "ویرایش فاکتور خرید")]
    UpdatePurchaseInvoice = 3,

    [Display(Name = "ویرایش فاکتور فروش")]
    UpdateSaleInvoice = 4,
}
