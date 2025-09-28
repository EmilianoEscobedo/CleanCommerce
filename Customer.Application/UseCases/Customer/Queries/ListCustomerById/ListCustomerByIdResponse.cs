using Customer.Application.DTOs;

namespace Customer.Application.UseCases.Customer.Queries.ListCustomerById;

public class ListCustomerByIdResponse
{
    public CustomerDto Customer { get; }

    public ListCustomerByIdResponse(CustomerDto customer)
    {
        Customer = customer;
    }
}