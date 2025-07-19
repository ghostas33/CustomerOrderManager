namespace CustomerOrderManager.Models.DTOs;

// responses
public class OrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public CustomerDto? Customer { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

// requests
public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
}

public class UpdateOrderDto
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();

}

public class CreateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
