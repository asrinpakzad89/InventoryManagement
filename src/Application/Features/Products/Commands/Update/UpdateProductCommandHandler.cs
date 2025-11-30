
namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IEFRepository<Product> _repository;
    private readonly IValidator<UpdateProductCommand> _validator;
    private readonly IMapper _mapper;
    private UpdateProductCommand _command;
    public UpdateProductCommandHandler(IEFRepository<Product> repository, IValidator<UpdateProductCommand> validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _command = new();
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _command = request;

        var validationResult = await _validator.ValidateAsync(_command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Correct();

        await UpdateProduct();
        return Unit.Value;
    }
    private void Correct()
    {
        _command.Code = (!string.IsNullOrEmpty(_command.Code) ? _command.Code.Replace("'", "") : "");
        _command.Name = (!string.IsNullOrEmpty(_command.Name) ? _command.Name.Replace("'", "") : "");
    }

    private async Task<ProductIdDto?> UpdateProduct()
    {
        var product = _mapper.Map<Product>(_command);
        var result = await _repository.UpdateAsync(product);
        await _repository.SaveChangeAsync();

        return _mapper.Map<ProductIdDto>(result);
    }
}
