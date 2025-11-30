namespace Application.Features.Products.Commands.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IEFRepository<Product> _repository;
    private DeleteProductCommand _command;
    public DeleteProductCommandHandler(IEFRepository<Product> repository)
    {
        _repository = repository;
        _command = new();
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _command = request;

        var product = await _repository.GetAsync(_command.Id, cancellationToken);
        if (product == null)
            throw new ValidationException("لطفا شناسه را درست وارد نمایید.");

        product.IsDelete = true;
        var result = await _repository.UpdateAsync(product);
        await _repository.SaveChangeAsync(cancellationToken);

        return Unit.Value;
    }
}
