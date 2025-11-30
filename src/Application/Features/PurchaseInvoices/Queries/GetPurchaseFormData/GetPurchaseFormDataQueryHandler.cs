namespace Application.Features.PurchaseInvoices.Queries.GetPurchaseFormData;

public class GetPurchaseFormDataQueryHandler : IRequestHandler<GetPurchaseFormDataQuery, PurchaseFormDataDto>
{
    private readonly IEFRepository<Product> _productRepository;
    private readonly IEFRepository<Supplier> _suppliersRepository;
    private readonly IMapper _mapper;
    public GetPurchaseFormDataQueryHandler(IEFRepository<Product> productRepository, IEFRepository<Supplier> suppliersRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _suppliersRepository = suppliersRepository;
        _mapper = mapper;
    }
    public async Task<PurchaseFormDataDto> Handle(GetPurchaseFormDataQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var productsDto = _mapper.Map<List<ProductDto>>(products);

        var suppliers = await _suppliersRepository.GetAllAsync(cancellationToken);
        var suppliersDto = _mapper.Map<List<SupplierDto>>(suppliers);

        return new PurchaseFormDataDto
        {
            Products = productsDto,
            Suppliers = suppliersDto
        };
    }
}

