using MediatR;
using Microsoft.AspNetCore.Mvc;
using Customer.Application.DTOs;
using Customer.Application.UseCases.Customer.Queries.ListCustomers;
using Customer.Application.UseCases.Customer.Queries.ListCustomerById;
using Customer.Application.UseCases.Customer.Commands.CreateCustomer;
using Customer.Application.UseCases.Customer.Commands.UpdateCustomer;
using Customer.Application.UseCases.Customer.Commands.DeleteCustomer;

namespace Customer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ListCustomersResponse>> GetAllCustomers()
    {
        _logger.LogInformation("Fetching all customers");
        var result = await _mediator.Send(new ListCustomersQuery());

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to fetch customers: {Errors}", result.Errors);
            return BadRequest(result.Errors);
        }

        return Ok(result.Value.Customers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ListCustomerByIdResponse>> GetCustomerById(int id)
    {
        _logger.LogInformation("Fetching customer with Id {Id}", id);
        var result = await _mediator.Send(new ListCustomerByIdQuery(id));

        if (result.IsFailure)
        {
            _logger.LogWarning("Customer with Id {Id} not found", id);
            return NotFound(result.Errors);
        }

        return Ok(result.Value.Customer);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCustomerResponse>> CreateCustomer([FromBody] CreateCustomerRequestDto createCustomerDto)
    {
        _logger.LogInformation("Creating new customer {@Customer}", createCustomerDto);
        var result = await _mediator.Send(new CreateCustomerCommand(createCustomerDto));

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to create customer: {Errors}", result.Errors);
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetCustomerById), new { id = result.Value.Customer.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateCustomerResponse>> UpdateCustomer(int id, [FromBody] UpdateCustomerRequestDto updateCustomerDto)
    {
        _logger.LogInformation("Updating customer with Id {Id}", id);
        var result = await _mediator.Send(new UpdateCustomerCommand(id, updateCustomerDto));

        if (result.IsFailure)
        {
            if (result.Errors.Any(e => e.Contains("not found")))
            {
                _logger.LogWarning("Customer with Id {Id} not found for update", id);
                return NotFound(result.Errors);
            }

            _logger.LogWarning("Failed to update customer with Id {Id}: {Errors}", id, result.Errors);
            return BadRequest(result.Errors);
        }

        return Ok(result.Value.Customer);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        _logger.LogInformation("Deleting customer with Id {Id}", id);
        var result = await _mediator.Send(new DeleteCustomerCommand(id));

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to delete customer with Id {Id}: {Errors}", id, result.Errors);
            return NotFound(result.Errors);
        }

        return NoContent();
    }
}
