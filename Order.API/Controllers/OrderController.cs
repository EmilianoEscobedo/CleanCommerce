using Microsoft.AspNetCore.Mvc;
using Order.Application.DTOs;
using Order.Application.Services;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        if (result.IsFailure)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var result = await _orderService.CreateOrderAsync(createOrderDto);
        if (result.IsFailure)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetOrderById), new { id = result.Value.Id }, result.Value);
    }
}