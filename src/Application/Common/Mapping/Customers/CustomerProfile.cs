namespace Application.Common.Mapping.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer,CustomerDto>().ReverseMap(); 
    }
}
