using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Update;

namespace Application.Common.Mapping.Products;

public class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product,ProductDto>()
            .ReverseMap();
        
        CreateMap<Product, CreateProductCommand>()
            .ReverseMap();

        CreateMap<Product, UpdateProductCommand>()
            .ReverseMap();

        CreateMap<ProductDto, CreateProductCommand>()
        .ReverseMap();

        CreateMap<ProductDto, UpdateProductCommand>()
            .ReverseMap();

        CreateMap<Product, ProductIdDto>()
            .ReverseMap();
    }
}
