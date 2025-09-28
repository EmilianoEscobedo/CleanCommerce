using Customer.Application.DTOs;

namespace Customer.Application.UseCases.Customer.Commands.CreateCustomer;

public class CreateCustomerResponse
{
    public CustomerDto Customer { get; }

    public CreateCustomerResponse(CustomerDto customer)
    {
        Customer = customer;
    }
}