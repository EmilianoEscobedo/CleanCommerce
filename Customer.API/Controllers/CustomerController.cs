using Microsoft.AspNetCore.Mvc;
using Customer.Application.DTOs;
using Customer.Application.Services;

namespace Customer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllProducts()
    {
        var result = await _customerService.GetAllCustomersAsync();

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerRequestDto createCustomerDto)
    {
        var result = await _customerService.CreateCustomerAsync(createCustomerDto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        return CreatedAtAction(nameof(GetCustomerById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] UpdateCustomerRequestDto updateCustomerDto)
    {
        var result = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
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
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return NoContent();
    }
}