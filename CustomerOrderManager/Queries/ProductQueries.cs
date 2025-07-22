using MediatR;
using CustomerOrderManager.Model.DTO;

namespace CustomerOrderManager.Queries;

/// <summary>
/// Query to get all products
/// </summary>
public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
{
}

/// <summary>
/// Query to get a product by ID
/// </summary>
public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public int Id { get; set; }
}