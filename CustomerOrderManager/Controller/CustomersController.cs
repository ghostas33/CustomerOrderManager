using Microsoft.AspNetCore.Mvc;
using CustomerOrderManager.Repository;
using CustomerOrderManager.Models.Entities;

namespace CustomerOrderManager.Controller;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customers = await _unitOfWork.Customers.FindAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _unitOfWork.Customers.FindByIdAsync(id);
        if (customer == null)
            return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        await _unitOfWork.Customers.SaveAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
    {
        if (id != customer.Id)
            return BadRequest();

        await _unitOfWork.Customers.SaveAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _unitOfWork.Customers.DeleteByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<Customer>> SearchByName([FromQuery] string firstName, [FromQuery] string lastName)
    {
        var customer = await _unitOfWork.Customers.FindByNameAsync(firstName, lastName);
        if (customer == null)
            return NotFound();
        return Ok(customer);
    }
}