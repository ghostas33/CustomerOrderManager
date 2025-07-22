using MediatR;
using CustomerOrderManager.Model.DTO;

namespace CustomerOrderManager.Commands;

/// <summary>
/// Command to create a new product
/// </summary>
public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// Command to update an existing product
/// </summary>
public class UpdateProductCommand : IRequest<ProductDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// Command to delete a product
/// </summary>
public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}