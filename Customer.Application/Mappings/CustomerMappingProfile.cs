using AutoMapper;
using Customer.Application.DTOs;

namespace Customer.Application.Mappings;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Domain.Entities.Customer, CustomerDto>();
        CreateMap<Domain.Entities.Address, AddressDto>();

        CreateMap<CreateCustomerRequestDto, Domain.Entities.Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

        CreateMap<UpdateCustomerRequestDto, Domain.Entities.Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

        CreateMap<AddressDto, Domain.Entities.Address>();
    }
}