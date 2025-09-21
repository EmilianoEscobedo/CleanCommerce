namespace Customer.Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Address { get; private set; }
    public DateTime RegistrationDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    public Customer(string name, string email, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be empty", nameof(email));

        Name = name;
        Email = email;
        Address = address;
        RegistrationDate = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string email, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be empty", nameof(email));

        Name = name;
        Email = email;
        Address = address;
        UpdatedDate = DateTime.UtcNow;
    }
}