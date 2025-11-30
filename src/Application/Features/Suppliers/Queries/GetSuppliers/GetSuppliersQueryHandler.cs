namespace Application.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<SupplierDto>>
{
    private readonly IEFRepository<Supplier> _repository;
    private readonly IMapper _mapper;
    public GetSuppliersQueryHandler(IEFRepository<Supplier> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {

        var result = await _repository.GetAllAsync(cancellationToken);
        var list = _mapper.Map<List<SupplierDto>>(result);

        return list;
    }
}
