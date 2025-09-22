using AutoMapper;
using FluentValidation;
using Product.Application.DTOs;
using Product.Domain.Common;
using Product.Domain.Repositories;

namespace Product.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;
    private readonly IValidator<UpdateProductStockDto> _updateStockValidator;

    public ProductService(
        IProductRepository productRepository,
        IMapper mapper,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator,
        IValidator<UpdateProductStockDto> updateStockValidator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _updateStockValidator = updateStockValidator;
    }

    public async Task<Result<ProductDto>> CreateProductAsync(CreateProductDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ProductDto>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        if (dto.Price < 0)
            return Result<ProductDto>.Failure("Price cannot be negative");

        if (dto.StockQuantity < 0)
            return Result<ProductDto>.Failure("Stock quantity cannot be negative");

        var product = new Domain.Entities.Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CreatedDate = DateTime.UtcNow
        };

        var addResult = await _productRepository.AddAsync(product);
        if (addResult.IsFailure)
            return Result<ProductDto>.Failure(addResult.Errors);

        var saveResult = await _productRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<ProductDto>.Failure(saveResult.Errors);

        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(product));
    }

    public async Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ProductDto>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        var productResult = await _productRepository.GetByIdAsync(id);
        if (productResult.IsFailure)
            return Result<ProductDto>.Failure(productResult.Errors);

        var product = productResult.Value;

        if (dto.Price < 0)
            return Result<ProductDto>.Failure("Price cannot be negative");

        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<ProductDto>.Failure("Name cannot be empty");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.UpdatedDate = DateTime.UtcNow;

        var updateResult = await _productRepository.UpdateAsync(product);
        if (updateResult.IsFailure)
            return Result<ProductDto>.Failure(updateResult.Errors);

        var saveResult = await _productRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<ProductDto>.Failure(saveResult.Errors);

        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(product));
    }

    public async Task<Result<ProductDto>> UpdateProductStockAsync(int id, UpdateProductStockDto dto)
    {
        var validation = await _updateStockValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ProductDto>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        var productResult = await _productRepository.GetByIdAsync(id);
        if (productResult.IsFailure)
            return Result<ProductDto>.Failure(productResult.Errors);

        var product = productResult.Value;

        if (product.StockQuantity + dto.Quantity < 0)
            return Result<ProductDto>.Failure("Cannot reduce stock below zero");

        product.StockQuantity += dto.Quantity;
        product.UpdatedDate = DateTime.UtcNow;

        var updateResult = await _productRepository.UpdateAsync(product);
        if (updateResult.IsFailure)
            return Result<ProductDto>.Failure(updateResult.Errors);

        var saveResult = await _productRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<ProductDto>.Failure(saveResult.Errors);

        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(product));
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var result = await _productRepository.GetAllAsync();
        if (result.IsFailure)
            return Result<IEnumerable<ProductDto>>.Failure(result.Errors);

        return Result<IEnumerable<ProductDto>>.Success(_mapper.Map<IEnumerable<ProductDto>>(result.Value));
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int id)
    {
        var result = await _productRepository.GetByIdAsync(id);
        if (result.IsFailure)
            return Result<ProductDto>.Failure(result.Errors);

        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(result.Value));
    }

    public async Task<Result<bool>> DeleteProductAsync(int id)
    {
        var existingResult = await _productRepository.GetByIdAsync(id);
        if (existingResult.IsFailure)
            return Result<bool>.Failure(existingResult.Errors);

        var deleteResult = await _productRepository.DeleteAsync(id);
        if (deleteResult.IsFailure)
            return Result<bool>.Failure(deleteResult.Errors);

        var saveResult = await _productRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<bool>.Failure(saveResult.Errors);

        return Result<bool>.Success(true);
    }
}