using Microsoft.AspNetCore.Mvc;
using MediatR;
using CustomerOrderManager.Commands;
using CustomerOrderManager.Queries;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Service.Interfaces;
using AutoMapper;

namespace CustomerOrderManager.Controller;

/// <summary>
/// Controller for managing customers
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomersController(IMediator mediator, ICustomerService customerService, IMapper mapper)
    {
        _mediator = mediator;
        _customerService = customerService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    /// <returns>List of customers</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var query = new GetAllCustomersQuery();
        var customers = await _mediator.Send(query);
        return Ok(customers);
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var query = new GetCustomerByIdQuery { Id = id };
        var customer = await _mediator.Send(query);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="customerDto">Customer data</param>
    /// <returns>Created customer</returns>
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto customerDto)
    {
        // Validate business rules
        var validationResult = await _customerService.ValidateCustomerDataAsync(customerDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateCustomerCommand>(customerDto);
        var createdCustomer = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="customerDto">Updated customer data</param>
    /// <returns>Updated customer</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, UpdateCustomerDto customerDto)
    {
        var command = _mapper.Map<UpdateCustomerCommand>(customerDto);
        command.Id = id;
        
        try
        {
            var updatedCustomer = await _mediator.Send(command);
            return Ok(updatedCustomer);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        // Check if customer can be deleted
        var canDelete = await _customerService.CanDeleteCustomerAsync(id);
        if (!canDelete)
        {
            return BadRequest("Customer cannot be deleted because they have existing orders");
        }

        var command = new DeleteCustomerCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    /// <summary>
    /// Search customers by name
    /// </summary>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <returns>Matching customers</returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomer([FromQuery] string firstName, [FromQuery] string lastName)
    {
        var query = new SearchCustomersByNameQuery { FirstName = firstName, LastName = lastName };
        var customers = await _mediator.Send(query);
        return Ok(customers);
    }

    /// <summary>
    /// Get customer statistics
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer statistics</returns>
    [HttpGet("{id}/statistics")]
    public async Task<ActionResult<CustomerStatisticsDto>> GetCustomerStatistics(int id)
    {
        var statistics = await _customerService.GetCustomerStatisticsAsync(id);
        return Ok(statistics);
    }
}