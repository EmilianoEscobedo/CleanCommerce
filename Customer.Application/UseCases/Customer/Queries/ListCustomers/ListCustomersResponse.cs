using Customer.Application.DTOs;

namespace Customer.Application.UseCases.Customer.Queries.ListCustomers;

public class ListCustomersResponse
{
    public IEnumerable<CustomerDto> Customers { get; }

    public ListCustomersResponse(IEnumerable<CustomerDto> customers)
    {
        Customers = customers;
    }
}