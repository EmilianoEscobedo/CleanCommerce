using MediatR;
using Customer.Domain.Common;
using Customer.Domain.Repositories;

namespace Customer.Application.UseCases.Customer.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result<DeleteCustomerResponse>>
{
    private readonly ICustomerRepository _repository;

    public DeleteCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<DeleteCustomerResponse>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var existingResult = await _repository.GetByIdAsync(request.Id);
        if (existingResult.IsFailure)
            return Result<DeleteCustomerResponse>.Failure(existingResult.Errors);

        var deleteResult = await _repository.DeleteAsync(request.Id);
        if (deleteResult.IsFailure)
            return Result<DeleteCustomerResponse>.Failure(deleteResult.Errors);

        var saveResult = await _repository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<DeleteCustomerResponse>.Failure(saveResult.Errors);

        return Result<DeleteCustomerResponse>.Success(new DeleteCustomerResponse(true));
    }
}