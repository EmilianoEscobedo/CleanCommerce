using WebClient.Application.DTOs.Order;
using WebClient.Domain.Common;

namespace WebClient.Application.Services;
    
public interface IOrderService
{
    Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderRequestDto orderDto);
    Task<Result<OrderResponseDto>> GetOrderByIdAsync(int id);
    Task<Result<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync();
}