using MediatR;
using CustomerOrderManager.Model.DTO;

namespace CustomerOrderManager.Commands;

/// <summary>
/// Command to create a new order
/// </summary>
public class CreateOrderCommand : IRequest<OrderDto>
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
}

/// <summary>
/// Command to update an existing order
/// </summary>
public class UpdateOrderCommand : IRequest<OrderDto>
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
}

/// <summary>
/// Command to delete an order
/// </summary>
public class DeleteOrderCommand : IRequest<bool>
{
    public int Id { get; set; }
}