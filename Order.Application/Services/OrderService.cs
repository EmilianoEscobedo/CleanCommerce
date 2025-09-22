using AutoMapper;
using FluentValidation;
using Order.Application.DTOs;
using Order.Domain.Common;
using Order.Domain.Repositories;

namespace Order.Application.Services;
using Domain.Entities;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrderDto> _createOrderValidator;
    private readonly IExternalServicesClient _externalServicesClient;

    public OrderService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateOrderDto> createOrderValidator,
        IExternalServicesClient externalServicesClient)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createOrderValidator = createOrderValidator;
        _externalServicesClient = externalServicesClient;
    }

    public async Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderDto orderDto)
    {
        var validationResult = await _createOrderValidator.ValidateAsync(orderDto);
        if (!validationResult.IsValid)
            return Result<OrderResponseDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));

        var customerResult = await _externalServicesClient.GetCustomerAsync(orderDto.CustomerId);
        if (customerResult.IsFailure)
            return Result<OrderResponseDto>.Failure("Customer not found");

        var order = _mapper.Map<Order>(orderDto);
        order.CustomerName = customerResult.Value.Name;

        var mappedItems = new List<OrderItem>();
        foreach (var itemDto in orderDto.Items)
        {
            var productResult = await _externalServicesClient.GetProductAsync(itemDto.ProductId);
            if (productResult.IsFailure)
                return Result<OrderResponseDto>.Failure($"Product {itemDto.ProductId} not found");

            var product = productResult.Value;
            var quantity = Math.Min(itemDto.Quantity, product.StockQuantity);

            if (quantity <= 0)
                return Result<OrderResponseDto>.Failure($"Invalid quantity for product {product.Id}");

            var orderItem = _mapper.Map<OrderItem>(itemDto);
            orderItem.ProductName = product.Name;
            orderItem.UnitPrice = product.Price;
            orderItem.Quantity = quantity;
            orderItem.CalculateSubtotal();
            mappedItems.Add(orderItem);

            var stockUpdate = await _externalServicesClient.UpdateProductStockAsync(product.Id, -quantity);
            if (stockUpdate.IsFailure)
                return Result<OrderResponseDto>.Failure($"Failed to update stock for product {product.Id}");
        }

        foreach (var item in mappedItems)
            order.AddItem(item);

        var createdOrderResult = await _unitOfWork.OrderRepository.AddAsync(order);
        if (createdOrderResult.IsFailure)
            return Result<OrderResponseDto>.Failure(createdOrderResult.Errors);

        await _unitOfWork.SaveChangesAsync();

        var orderResponseDto = _mapper.Map<OrderResponseDto>(createdOrderResult.Value);
        return Result<OrderResponseDto>.Success(orderResponseDto);
    }

    public async Task<Result<OrderResponseDto>> GetOrderByIdAsync(int id)
    {
        var orderResult = await _unitOfWork.OrderRepository.GetByIdAsync(id);
        if (orderResult.IsFailure)
            return Result<OrderResponseDto>.Failure(orderResult.Errors);

        return Result<OrderResponseDto>.Success(_mapper.Map<OrderResponseDto>(orderResult.Value));
    }

    public async Task<Result<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync()
    {
        var ordersResult = await _unitOfWork.OrderRepository.GetAllAsync();
        if (ordersResult.IsFailure)
            return Result<IEnumerable<OrderResponseDto>>.Failure(ordersResult.Errors);

        var orderDtos = _mapper.Map<IEnumerable<OrderResponseDto>>(ordersResult.Value);
        return Result<IEnumerable<OrderResponseDto>>.Success(orderDtos);
    }
}
