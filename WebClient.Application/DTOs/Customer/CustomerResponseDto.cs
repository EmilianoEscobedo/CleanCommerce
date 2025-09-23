
namespace WebClient.Application.DTOs.Customer;

public class CustomerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public AddressDto Address { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}