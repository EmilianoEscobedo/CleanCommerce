using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTOs;
using Product.Application.UseCases.Product.Commands.DeleteProduct;
using Product.Application.UseCases.Product.Commands.UpdateProduct;
using Product.Application.UseCases.Product.Commands.UpdateProductStock;
using Product.Application.UseCases.Product.Queries.ListProductById;
using Product.Application.UseCases.Product.Queries.ListProducts;
using ILogger = Serilog.ILogger;
using Serilog;

namespace Product.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ILogger _logger;

    public ProductController(ISender mediator)
    {
        _mediator = mediator;
        _logger = Log.ForContext<ProductController>();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        _logger.Information("Fetching all products");
        var result = await _mediator.Send(new ListProductsQuery());
        if (result.IsFailure)
        {
            _logger.Warning("Failed to fetch products: {Errors}", result.Errors);
            return BadRequest(result.Errors);
        }
        _logger.Information("Fetched {Count} products", result.Value.Products.Count());
        return Ok(result.Value.Products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        _logger.Information("Fetching product with id {ProductId}", id);
        var result = await _mediator.Send(new ListProductByIdQuery(id));
        if (result.IsFailure)
        {
            _logger.Warning("Product with id {ProductId} not found", id);
            return NotFound(result.Errors);
        }
        _logger.Information("Product with id {ProductId} fetched successfully", id);
        return Ok(result.Value.Product);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        _logger.Information("Creating product {@Product}", createProductDto);
        var result = await _mediator.Send(new CreateProductCommand(createProductDto));
        if (result.IsFailure)
        {
            _logger.Warning("Failed to create product: {Errors}", result.Errors);
            return BadRequest(result.Errors);
        }
        _logger.Information("Product {ProductId} created successfully", result.Value.Product.Id);
        return CreatedAtAction(nameof(GetProductById), new { id = result.Value.Product.Id }, result.Value.Product);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        _logger.Information("Updating product {ProductId}", id);
        var result = await _mediator.Send(new UpdateProductCommand(id, updateProductDto));
        if (result.IsFailure)
        {
            _logger.Warning("Failed to update product {ProductId}: {Errors}", id, result.Errors);
            if (result.Errors.Any(e => e.Contains("not found")))
                return NotFound(result.Errors);
            return BadRequest(result.Errors);
        }
        _logger.Information("Product {ProductId} updated successfully", id);
        return Ok(result.Value.Product);
    }

    [HttpPatch("{id}/stock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> UpdateProductStock(int id, [FromBody] UpdateProductStockDto updateProductStockDto)
    {
        _logger.Information("Updating stock for product {ProductId}", id);
        var result = await _mediator.Send(new UpdateProductStockCommand(id, updateProductStockDto));
        if (result.IsFailure)
        {
            _logger.Warning("Failed to update stock for product {ProductId}: {Errors}", id, result.Errors);
            if (result.Errors.Any(e => e.Contains("not found")))
                return NotFound(result.Errors);
            return BadRequest(result.Errors);
        }
        _logger.Information("Stock updated successfully for product {ProductId}", id);
        return Ok(result.Value.Product);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        _logger.Information("Deleting product {ProductId}", id);
        var result = await _mediator.Send(new DeleteProductCommand(id));
        if (result.IsFailure)
        {
            _logger.Warning("Failed to delete product {ProductId}: {Errors}", id, result.Errors);
            return NotFound(result.Errors);
        }
        _logger.Information("Product {ProductId} deleted successfully", id);
        return NoContent();
    }
}