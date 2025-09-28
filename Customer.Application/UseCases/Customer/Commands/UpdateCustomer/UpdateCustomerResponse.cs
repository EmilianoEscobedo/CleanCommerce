using Customer.Application.DTOs;

namespace Customer.Application.UseCases.Customer.Commands.UpdateCustomer;

public class UpdateCustomerResponse
{
    public CustomerDto Customer { get; }

    public UpdateCustomerResponse(CustomerDto customer)
    {
        Customer = customer;
    }
}