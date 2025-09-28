using Product.Application.DTOs;

namespace Product.Application.UseCases.Product.Queries.ListProducts;
public class ListProductsResponse
{
    public IEnumerable<ProductDto> Products { get; }

    public ListProductsResponse(IEnumerable<ProductDto> products)
    {
        Products = products;
    }
}