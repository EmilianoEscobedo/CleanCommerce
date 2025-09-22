namespace Order.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items;

    public Order()
    {
        _items = new List<OrderItem>();
        OrderDate = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public decimal Total { get; set; }
    public DateTime OrderDate { get; set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public void AddItem(OrderItem item)
    {
        _items.Add(item);
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        Total = _items.Sum(item => item.Subtotal);
    }
}