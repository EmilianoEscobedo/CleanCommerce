namespace Customer.Application.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateCustomerDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Address { get; set; }
}

public class UpdateCustomerDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}