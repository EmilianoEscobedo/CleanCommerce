namespace Product.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    public Product(string name, string? description, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        if (price < 0)
            throw new ArgumentException("Product price cannot be negative", nameof(price));

        if (stockQuantity < 0)
            throw new ArgumentException("Product stock quantity cannot be negative", nameof(stockQuantity));

        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        CreatedDate = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        if (price < 0)
            throw new ArgumentException("Product price cannot be negative", nameof(price));

        Name = name;
        Description = description;
        Price = price;
        UpdatedDate = DateTime.UtcNow;
    }

    public void UpdateStock(int quantity)
    {
        if (StockQuantity + quantity < 0)
            throw new ArgumentException("Cannot reduce stock below zero", nameof(quantity));

        StockQuantity += quantity;
        UpdatedDate = DateTime.UtcNow;
    }

    public bool HasSufficientStock(int requestedQuantity)
    {
        return StockQuantity >= requestedQuantity;
    }

    public int GetAvailableStock(int requestedQuantity)
    {
        return Math.Min(StockQuantity, requestedQuantity);
    }
}