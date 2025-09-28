using AutoMapper;
using FluentValidation;
using MediatR;
using Order.Application.DTOs;
using Order.Domain.Common;
using Order.Domain.Repositories;
using Order.Application.Services;
using Order.Domain.Entities;

namespace Order.Application.UseCases.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrderRequestDto> _validator;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateOrderRequestDto> validator,
        ICustomerService customerService,
        IProductService productService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _customerService = customerService;
        _productService = productService;
    }

    public async Task<Result<CreateOrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request.Order, cancellationToken);
        if (!validation.IsValid)
            return Result<CreateOrderResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var customerResult = await _customerService.GetCustomerAsync(request.Order.CustomerId);
        if (customerResult.IsFailure)
            return Result<CreateOrderResponse>.Failure("Customer not found");

        var order = _mapper.Map<Domain.Entities.Order>(request.Order);
        order.CustomerName = customerResult.Value.Name;

        var mappedItems = new List<OrderItem>();
        foreach (var itemDto in request.Order.Items)
        {
            var productResult = await _productService.GetProductAsync(itemDto.ProductId);
            if (productResult.IsFailure)
                return Result<CreateOrderResponse>.Failure($"Product {itemDto.ProductId} not found");

            var product = productResult.Value;
            var quantity = Math.Min(itemDto.Quantity, product.StockQuantity);

            if (quantity <= 0)
                continue;

            var orderItem = _mapper.Map<OrderItem>(itemDto);
            orderItem.ProductName = product.Name;
            orderItem.UnitPrice = product.Price;
            orderItem.Quantity = quantity;
            orderItem.CalculateSubtotal();
            mappedItems.Add(orderItem);

            var stockUpdate = await _productService.UpdateProductStockAsync(product.Id, -quantity);
            if (stockUpdate.IsFailure)
                return Result<CreateOrderResponse>.Failure($"Failed to update stock for product {product.Id}");
        }

        if (!mappedItems.Any())
            return Result<CreateOrderResponse>.Failure("No items with available stock to create the order");

        foreach (var item in mappedItems)
            order.AddItem(item);

        var createdOrderResult = await _unitOfWork.OrderRepository.AddAsync(order);
        if (createdOrderResult.IsFailure)
            return Result<CreateOrderResponse>.Failure(createdOrderResult.Errors);

        await _unitOfWork.SaveChangesAsync();

        var dto = _mapper.Map<OrderResponseDto>(createdOrderResult.Value);
        return Result<CreateOrderResponse>.Success(new CreateOrderResponse { Order = dto });
    }
}
