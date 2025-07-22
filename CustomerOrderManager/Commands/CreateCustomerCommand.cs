using MediatR;
using CustomerOrderManager.Model.DTO;

namespace CustomerOrderManager.Commands;

/// <summary>
/// Command to create a new customer
/// </summary>
public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

/// <summary>
/// Command to update an existing customer
/// </summary>
public class UpdateCustomerCommand : IRequest<CustomerDto>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

/// <summary>
/// Command to delete a customer
/// </summary>
public class DeleteCustomerCommand : IRequest<bool>
{
    public int Id { get; set; }
}