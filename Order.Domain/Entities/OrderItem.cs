namespace Order.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public Order Order { get; set; }

    public void CalculateSubtotal()
    {
        Subtotal = UnitPrice * Quantity;
    }
}