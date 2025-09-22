using AutoMapper;
using Order.Application.DTOs;
using Order.Domain.Entities;

namespace Order.Application.Mapping;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Domain.Entities.Order, OrderResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<OrderItem, OrderItemResponseDto>();
        
        CreateMap<CreateOrderDto, Domain.Entities.Order>()
            .ForMember(dest => dest.CustomerName, opt => opt.Ignore()) 
            .ForMember(dest => dest.Total, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<OrderItemDto, OrderItem>()
            .ForMember(dest => dest.Subtotal, opt => opt.Ignore());
    }
}