namespace Product.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateProductDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}

public class UpdateProductDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

public class UpdateProductStockDto
{
    public int Quantity { get; set; }
}