namespace WebClient.Application.DTOs.Order;

public class CreateOrderRequestDto
{
    public int CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}