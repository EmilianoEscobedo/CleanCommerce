using Order.Application.DTOs;

namespace Order.Application.UseCases.Order.Queries.ListOrders;

public class ListOrdersResponse
{
    public IEnumerable<OrderResponseDto> Orders { get; set; } = new List<OrderResponseDto>();
}