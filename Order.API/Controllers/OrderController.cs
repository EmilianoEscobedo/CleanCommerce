using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.DTOs;
using Order.Application.UseCases.Order.Commands.CreateOrder;
using Order.Application.UseCases.Order.Queries.ListOrderById;
using Order.Application.UseCases.Order.Queries.ListOrders;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ISender mediator, ILogger<OrderController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
    {
        _logger.LogInformation("Fetching all orders");
        var result = await _mediator.Send(new ListOrdersQuery());

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to fetch orders: {Errors}", string.Join(", ", result.Errors));
            return BadRequest(result.Errors);
        }

        _logger.LogInformation("Successfully fetched {Count} orders", result.Value.Orders.Count());
        return Ok(result.Value.Orders);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
    {
        _logger.LogInformation("Fetching order with ID {Id}", id);
        var result = await _mediator.Send(new ListOrderByIdQuery(id));

        if (result.IsFailure)
        {
            _logger.LogWarning("Order with ID {Id} not found", id);
            return NotFound(result.Errors);
        }

        _logger.LogInformation("Successfully fetched order with ID {Id}", id);
        return Ok(result.Value.Order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderRequestDto createOrderRequestDto)
    {
        _logger.LogInformation("Creating a new order for customer {CustomerId}", createOrderRequestDto.CustomerId);
        var result = await _mediator.Send(new CreateOrderCommand(createOrderRequestDto));

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to create order: {Errors}", string.Join(", ", result.Errors));
            return BadRequest(result.Errors);
        }

        _logger.LogInformation("Order created successfully with ID {Id}", result.Value.Order.Id);
        return CreatedAtAction(nameof(GetOrderById), new { id = result.Value.Order.Id }, result.Value.Order);
    }
}