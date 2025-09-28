namespace Customer.Domain.Entities;

public class Address
{
    public string Country { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public int Number { get; private set; }

    public Address(string country, string city, string street, int number)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;
    }
}