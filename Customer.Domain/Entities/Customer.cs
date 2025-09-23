namespace Customer.Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Address Address { get; private set; }
    public DateTime RegistrationDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    private Customer() { }

    public Customer(string name, string email, Address address)
    {
        Name = name;
        Email = email;
        Address = address;
        RegistrationDate = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string email, Address address)
    {
        Name = name;
        Email = email;
        Address = address;
        UpdatedDate = DateTime.UtcNow;
    }
}