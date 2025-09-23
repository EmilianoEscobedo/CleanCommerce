
namespace WebClient.Application.DTOs.Customer;

public class CreateCustomerRequestDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public AddressDto? Address { get; set; }
}