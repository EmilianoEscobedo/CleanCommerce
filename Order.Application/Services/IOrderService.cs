using Order.Application.DTOs;
using Order.Domain.Common;

namespace Order.Application.Services;

public interface IOrderService
{
    Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderDto orderDto);
    Task<Result<OrderResponseDto>> GetOrderByIdAsync(int id);
    Task<Result<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync();
}