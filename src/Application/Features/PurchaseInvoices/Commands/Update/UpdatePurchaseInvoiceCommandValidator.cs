namespace Application.Features.PurchaseInvoices.Commands.Update;

public class UpdatePurchaseInvoiceCommandValidator: AbstractValidator<UpdatePurchaseInvoiceCommand>
{
    public UpdatePurchaseInvoiceCommandValidator()
    {
        RuleFor(x=>x.Id)
            .NotEmpty().WithMessage("شناسه را وارد نمایید.");

        RuleFor(x => x.InvoiceNumber)
           .NotEmpty().WithMessage("شماره فاکتور را وارد نمایید.");

        RuleFor(x => x.SupplierId)
            .GreaterThan(0)
            .NotEmpty().WithMessage("تامین کننده را انتخاب نمایید.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("اقلام فاکتور را وارد نمایید");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
            .GreaterThan(0)
            .NotEmpty().WithMessage("محصول را انتخاب نمایید."); ;

            item.RuleFor(i => i.Quantity)
            .GreaterThan(0).NotEmpty().WithMessage("تعداد محصول را انتخاب نمایید."); ;
        });
    }
}
