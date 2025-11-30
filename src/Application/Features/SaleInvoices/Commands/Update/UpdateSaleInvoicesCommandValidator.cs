namespace Application.Features.SaleInvoices.Commands.Update;

public class UpdateSaleInvoicesCommandValidator : AbstractValidator<UpdateSaleInvoicesCommand>
{
    public UpdateSaleInvoicesCommandValidator()
    {
        RuleFor(x => x.InvoiceNumber)
           .NotEmpty().WithMessage("شماره فاکتور را وارد نمایید.");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .NotEmpty().WithMessage("مشتری را انتخاب نمایید.");

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
