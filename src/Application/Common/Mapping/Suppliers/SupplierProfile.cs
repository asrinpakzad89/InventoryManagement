namespace Application.Common.Mapping.Suppliers;

public class SupplierProfile : Profile
{
    public SupplierProfile()
    {
        CreateMap<Supplier, SupplierDto>().ReverseMap();
    }
}
