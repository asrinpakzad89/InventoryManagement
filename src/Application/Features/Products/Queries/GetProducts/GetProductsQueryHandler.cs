namespace Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IEFRepository<Product> _repository;
    private readonly IMapper _mapper;
    public GetProductsQueryHandler(IEFRepository<Product> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {

        var result = await _repository.GetAllAsync(cancellationToken);
        var list = _mapper.Map<List<ProductDto>>(result);

        return list;
    }
}
