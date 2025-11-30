namespace Application.Features.SaleInvoices.Queries.GetSaleFormData;

public class GetSaleFormDataQueryHandler : IRequestHandler<GetSaleFormDataQuery, SaleFormDataDto>
{
    private readonly IEFRepository<Product> _productRepository;
    private readonly IEFRepository<Customer> _customersRepository;
    private readonly IMapper _mapper;
    public GetSaleFormDataQueryHandler(IEFRepository<Product> productRepository, IEFRepository<Customer> customersRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _customersRepository = customersRepository;
        _mapper = mapper;
    }
    async Task<SaleFormDataDto> IRequestHandler<GetSaleFormDataQuery, SaleFormDataDto>.Handle(GetSaleFormDataQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var productsDto = _mapper.Map<List<ProductDto>>(products);

        var customers = await _customersRepository.GetAllAsync(cancellationToken);
        var customersDto = _mapper.Map<List<CustomerDto>>(customers);

        return new SaleFormDataDto
        {
            Products = productsDto,
            Customers = customersDto
        };
    }
}
