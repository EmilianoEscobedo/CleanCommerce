namespace Customer.Application.UseCases.Customer.Commands.DeleteCustomer;

public class DeleteCustomerResponse
{
    public bool Success { get; }

    public DeleteCustomerResponse(bool success)
    {
        Success = success;
    }
}