namespace Customer.Application.DTOs;

public class UpdateCustomerRequestDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public AddressDto Address { get; set; }
}