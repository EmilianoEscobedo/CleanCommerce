
using System.ComponentModel.DataAnnotations;

namespace WebClient.Application.DTOs.Customer;

public class CreateCustomerRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public required string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    public AddressDto? Address { get; set; }
}