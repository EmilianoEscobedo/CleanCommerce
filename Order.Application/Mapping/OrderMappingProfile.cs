using AutoMapper;
using Order.Application.DTOs;

namespace Order.Application.Mapping;
using Domain.Entities;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<OrderItem, OrderItemResponseDto>();
        
        CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.CustomerName, opt => opt.Ignore()) 
            .ForMember(dest => dest.Total, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<OrderItemDto, OrderItem>()
            .ForMember(dest => dest.Subtotal, opt => opt.Ignore());
    }
}