using Microsoft.AspNetCore.Mvc;
using CustomerOrderManager.Repository;
using CustomerOrderManager.Models.Entities;

namespace CustomerOrderManager.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var orders = await _unitOfWork.Orders.FindAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _unitOfWork.Orders.FindByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        await _unitOfWork.Orders.SaveAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, Order order)
    {
        if (id != order.Id)
            return BadRequest();

        await _unitOfWork.Orders.SaveAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _unitOfWork.Orders.DeleteByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomer(int customerId)
    {
        var orders = await _unitOfWork.Orders.FindByCustomerIdAsync(customerId);
        return Ok(orders);
    }

    // Απαίτηση άσκησης: Support για iteration by date
    [HttpGet("by-date-range")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDateRange(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var orders = await _unitOfWork.Orders.FindByDateRangeAsync(startDate, endDate);
        return Ok(orders);
    }
}