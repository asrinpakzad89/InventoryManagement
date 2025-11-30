namespace Application.Features.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("کد محصول را وارد نمایید.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام محصول را وارد نماید.")
            .MinimumLength(2).WithMessage("نام محصول حداقل باید 2 کاراکتر باشد.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("نوع محصول را انتخاب نمایید.");

        RuleFor(x => x.Crime)
           .NotEmpty().WithMessage("جرم محصول را وارد نمایید.");
    }
}
