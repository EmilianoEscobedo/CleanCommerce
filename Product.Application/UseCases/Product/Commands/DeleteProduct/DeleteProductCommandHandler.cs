using MediatR;
using Product.Domain.Common;
using Product.Domain.Repositories;

namespace Product.Application.UseCases.Product.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<DeleteProductResponse>>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<DeleteProductResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var existingResult = await _repository.GetByIdAsync(request.Id);
        if (existingResult.IsFailure)
            return Result<DeleteProductResponse>.Failure(existingResult.Errors);

        var deleteResult = await _repository.DeleteAsync(request.Id);
        if (deleteResult.IsFailure)
            return Result<DeleteProductResponse>.Failure(deleteResult.Errors);

        var saveResult = await _repository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<DeleteProductResponse>.Failure(saveResult.Errors);

        return Result<DeleteProductResponse>.Success(new DeleteProductResponse(true));
    }
}