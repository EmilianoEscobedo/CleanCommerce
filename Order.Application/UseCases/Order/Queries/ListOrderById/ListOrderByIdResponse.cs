using Order.Application.DTOs;

namespace Order.Application.UseCases.Order.Queries.ListOrderById;

public class ListOrderByIdResponse
{
    public OrderResponseDto Order { get; set; } = default!;
}