namespace Order.Application.DTOs;

public class CustomerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public AddressDto Address { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class AddressDto
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
}