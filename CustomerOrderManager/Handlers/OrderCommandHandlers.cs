using MediatR;
using AutoMapper;
using CustomerOrderManager.Commands;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Models.Entities;
using CustomerOrderManager.Repository;

namespace CustomerOrderManager.Handlers;

/// <summary>
/// Handler for creating a new order
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = request.OrderDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            OrderItems = new List<OrderItem>()
        };

        decimal totalPrice = 0;
        foreach (var itemDto in request.OrderItems)
        {
            var product = await _unitOfWork.Products.FindByIdAsync(itemDto.ProductId);
            if (product == null)
                throw new ArgumentException($"Product with ID {itemDto.ProductId} not found");

            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };
            order.OrderItems.Add(orderItem);
            totalPrice += orderItem.TotalPrice;
        }

        order.TotalPrice = totalPrice;

        var savedOrder = await _unitOfWork.Orders.SaveAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderDto>(savedOrder);
    }
}

/// <summary>
/// Handler for updating an existing order
/// </summary>
public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var existingOrder = await _unitOfWork.Orders.FindByIdAsync(request.Id);
        if (existingOrder == null)
            throw new ArgumentException($"Order with ID {request.Id} not found");

        existingOrder.CustomerId = request.CustomerId;
        existingOrder.OrderDate = request.OrderDate;
        existingOrder.UpdatedAt = DateTime.UtcNow;

        // Clear existing order items and add new ones
        existingOrder.OrderItems.Clear();
        decimal totalPrice = 0;
        foreach (var itemDto in request.OrderItems)
        {
            var product = await _unitOfWork.Products.FindByIdAsync(itemDto.ProductId);
            if (product == null)
                throw new ArgumentException($"Product with ID {itemDto.ProductId} not found");

            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };
            existingOrder.OrderItems.Add(orderItem);
            totalPrice += orderItem.TotalPrice;
        }

        existingOrder.TotalPrice = totalPrice;

        var updatedOrder = await _unitOfWork.Orders.SaveAsync(existingOrder);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderDto>(updatedOrder);
    }
}

/// <summary>
/// Handler for deleting an order
/// </summary>
public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.FindByIdAsync(request.Id);
        if (order == null)
            return false;

        await _unitOfWork.Orders.DeleteByIdAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}