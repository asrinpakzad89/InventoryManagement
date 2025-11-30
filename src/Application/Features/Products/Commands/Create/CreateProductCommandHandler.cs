using System.Threading;

namespace Application.Features.Products.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductIdDto>
{
    private readonly IEFRepository<Product> _repository;
    private readonly IValidator<CreateProductCommand> _validator;
    private readonly IMapper _mapper;
    private CreateProductCommand _command;
    public CreateProductCommandHandler(IEFRepository<Product> repository, IValidator<CreateProductCommand> validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _command = new();
        _mapper = mapper;
    }

    async Task<ProductIdDto> IRequestHandler<CreateProductCommand, ProductIdDto>.Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        _command = request;

        var validationResult = await _validator.ValidateAsync(_command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Correct();

        var result = await CreateProduct();
        return result!;
    }

    private void Correct()
    {
        _command.Code = (!string.IsNullOrEmpty(_command.Code) ? _command.Code.Replace("'", "") : "");
        _command.Name = (!string.IsNullOrEmpty(_command.Name) ? _command.Name.Replace("'", "") : "");
    }

    private async Task<ProductIdDto?> CreateProduct()
    {
        var product = _mapper.Map<Product>(_command);
        product.IsDisable = false;
        product.IsDelete = false;
        product.PurchasePrice = 0;
        product.SellingPrice = 0;
        product.Quantity = 0;

        var result = await _repository.AddAsync(product);
        await _repository.SaveChangeAsync();

        return _mapper.Map<ProductIdDto>(result);
    }
}
