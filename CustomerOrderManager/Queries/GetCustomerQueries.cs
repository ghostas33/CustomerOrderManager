using MediatR;
using CustomerOrderManager.Model.DTO;

namespace CustomerOrderManager.Queries;

/// <summary>
/// Query to get all customers
/// </summary>
public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
}

/// <summary>
/// Query to get a customer by ID
/// </summary>
public class GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    public int Id { get; set; }
}

/// <summary>
/// Query to search customers by name
/// </summary>
public class SearchCustomersByNameQuery : IRequest<IEnumerable<CustomerDto>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}