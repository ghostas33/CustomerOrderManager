using MediatR;
using CustomerOrderManager.Model.DTO;

namespace CustomerOrderManager.Queries;

/// <summary>
/// Query to get all orders
/// </summary>
public class GetAllOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
}

/// <summary>
/// Query to get an order by ID
/// </summary>
public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public int Id { get; set; }
}

/// <summary>
/// Query to get orders by customer ID
/// </summary>
public class GetOrdersByCustomerIdQuery : IRequest<IEnumerable<OrderDto>>
{
    public int CustomerId { get; set; }
}

/// <summary>
/// Query to get orders within a date range
/// </summary>
public class GetOrdersByDateRangeQuery : IRequest<IEnumerable<OrderDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}