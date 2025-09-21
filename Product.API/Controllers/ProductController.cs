using Microsoft.AspNetCore.Mvc;
using Product.Application.DTOs;
using Product.Application.Services;

namespace Product.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var result = await _productService.CreateProductAsync(createProductDto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        return CreatedAtAction(nameof(GetProductById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        var result = await _productService.UpdateProductAsync(id, updateProductDto);
        if (!result.IsSuccess)
        {
            if (result.Errors.Any(e => e.Contains("not found")))
                return NotFound(result.Errors);
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{id}/stock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> UpdateProductStock(int id, [FromBody] UpdateProductStockDto updateProductStockDto)
    {
        var result = await _productService.UpdateProductStockAsync(id, updateProductStockDto);
        if (!result.IsSuccess)
        {
            if (result.Errors.Any(e => e.Contains("not found")))
                return NotFound(result.Errors);
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return NoContent();
    }
}