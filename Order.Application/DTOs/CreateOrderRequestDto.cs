namespace Order.Application.DTOs;

public class CreateOrderRequestDto
{
    public int CustomerId { get; set; }
    public List<CreateOrderItemRequestDto> Items { get; set; }
}

public class CreateOrderItemRequestDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}