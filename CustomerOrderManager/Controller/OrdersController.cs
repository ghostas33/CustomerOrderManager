using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using CustomerOrderManager.Commands;
using CustomerOrderManager.Queries;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Service.Interfaces;

namespace CustomerOrderManager.Controller;

/// <summary>
/// Orders management controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IMediator mediator, IOrderService orderService, IMapper mapper)
    {
        _mediator = mediator;
        _orderService = orderService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns>List of orders</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var query = new GetAllOrdersQuery();
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Order details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var order = await _mediator.Send(query);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="orderDto">Order data</param>
    /// <returns>Created order</returns>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto orderDto)
    {
        // Validate business rules
        var validationResult = await _orderService.ValidateOrderDataAsync(orderDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateOrderCommand>(orderDto);
        var createdOrder = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
    }

    /// <summary>
    /// Update an existing order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="orderDto">Updated order data</param>
    /// <returns>Updated order</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<OrderDto>> UpdateOrder(int id, UpdateOrderDto orderDto)
    {
        // Check if order can be modified
        var canModify = await _orderService.CanModifyOrderAsync(id);
        if (!canModify)
        {
            return BadRequest("Order cannot be modified after 24 hours of creation");
        }

        var command = _mapper.Map<UpdateOrderCommand>(orderDto);
        command.Id = id;
        
        try
        {
            var updatedOrder = await _mediator.Send(command);
            return Ok(updatedOrder);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Delete an order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var command = new DeleteOrderCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    /// <summary>
    /// Get orders by customer ID
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>Customer orders</returns>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomer(int customerId)
    {
        var query = new GetOrdersByCustomerIdQuery { CustomerId = customerId };
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    /// <summary>
    /// Get orders by date range - Support για iteration by date
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Orders in date range</returns>
    [HttpGet("by-date-range")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByDateRange(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var query = new GetOrdersByDateRangeQuery { StartDate = startDate, EndDate = endDate };
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    /// <summary>
    /// Get order statistics for a date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Order statistics</returns>
    [HttpGet("statistics")]
    public async Task<ActionResult<OrderStatisticsDto>> GetOrderStatistics(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var statistics = await _orderService.GetOrderStatisticsAsync(startDate, endDate);
        return Ok(statistics);
    }
}