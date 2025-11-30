namespace Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IEFRepository<Product> _repository;
    private readonly IMapper _mapper;
    private GetProductByIdQuery _query;
    public GetProductByIdQueryHandler(IEFRepository<Product> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _query = new();
    }
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _query = request;

        var result = await _repository.GetAsync(_query.Id, cancellationToken);
        var list = _mapper.Map<ProductDto>(result);

        return list;
    }
}
