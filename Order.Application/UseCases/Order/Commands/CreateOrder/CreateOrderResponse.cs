using Order.Application.DTOs;

namespace Order.Application.UseCases.Order.Commands.CreateOrder;

public class CreateOrderResponse
{
    public OrderResponseDto Order { get; set; } = default!;
}