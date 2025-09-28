using AutoMapper;
using FluentValidation;
using MediatR;
using Product.Application.DTOs;
using Product.Domain.Common;
using Product.Domain.Repositories;

namespace Product.Application.UseCases.Product.Commands.UpdateProductStock;

public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, Result<UpdateProductStockResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateProductStockDto> _validator;

    public UpdateProductStockCommandHandler(IProductRepository repository, IMapper mapper, IValidator<UpdateProductStockDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<UpdateProductStockResponse>> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var validation = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return Result<UpdateProductStockResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        var productResult = await _repository.GetByIdAsync(request.Id);
        if (productResult.IsFailure)
            return Result<UpdateProductStockResponse>.Failure(productResult.Errors);

        var product = productResult.Value;

        if (product.StockQuantity + dto.Quantity < 0)
            return Result<UpdateProductStockResponse>.Failure("Cannot reduce stock below zero");

        product.StockQuantity += dto.Quantity;
        product.UpdatedDate = DateTime.UtcNow;

        var updateResult = await _repository.UpdateAsync(product);
        if (updateResult.IsFailure)
            return Result<UpdateProductStockResponse>.Failure(updateResult.Errors);

        var saveResult = await _repository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<UpdateProductStockResponse>.Failure(saveResult.Errors);

        return Result<UpdateProductStockResponse>.Success(new UpdateProductStockResponse(_mapper.Map<ProductDto>(product)));
    }
}
