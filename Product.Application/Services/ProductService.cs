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

    public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Result<IEnumerable<ProductDto>>.Success(productDtos);
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return Result<ProductDto>.Failure($"Product with id {id} not found");
        }

        var productDto = _mapper.Map<ProductDto>(product);
        return Result<ProductDto>.Success(productDto);
    }

    public async Task<Result<ProductDto>> CreateProductAsync(CreateProductDto createProductDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createProductDto);
        if (!validationResult.IsValid)
        {
            return Result<ProductDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var product = new Domain.Entities.Product(
            createProductDto.Name, 
            createProductDto.Description, 
            createProductDto.Price, 
            createProductDto.StockQuantity
        );

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(product);
        return Result<ProductDto>.Success(productDto);
    }

    public async Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateProductDto);
        if (!validationResult.IsValid)
        {
            return Result<ProductDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return Result<ProductDto>.Failure($"Product with id {id} not found");
        }

        existingProduct.UpdateDetails(
            updateProductDto.Name,
            updateProductDto.Description,
            updateProductDto.Price
        );

        await _productRepository.UpdateAsync(existingProduct);
        await _productRepository.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(existingProduct);
        return Result<ProductDto>.Success(productDto);
    }

    public async Task<Result<ProductDto>> UpdateProductStockAsync(int id, UpdateProductStockDto updateProductStockDto)
    {
        var validationResult = await _updateStockValidator.ValidateAsync(updateProductStockDto);
        if (!validationResult.IsValid)
        {
            return Result<ProductDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return Result<ProductDto>.Failure($"Product with id {id} not found");
        }

        var currentStock = existingProduct.StockQuantity;
        var newStock = updateProductStockDto.Quantity;
        var stockDifference = newStock - currentStock;
        
        existingProduct.UpdateStock(stockDifference);

        await _productRepository.UpdateAsync(existingProduct);
        await _productRepository.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(existingProduct);
        return Result<ProductDto>.Success(productDto);
    }

    public async Task<Result<bool>> DeleteProductAsync(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return Result<bool>.Failure($"Product with id {id} not found");
        }

        await _productRepository.DeleteAsync(id);
        await _productRepository.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}