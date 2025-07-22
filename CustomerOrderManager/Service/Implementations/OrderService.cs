using CustomerOrderManager.Commands;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Repository;
using CustomerOrderManager.Service.Interfaces;
using ValidationResult = CustomerOrderManager.Service.Interfaces.ValidationResult;

namespace CustomerOrderManager.Service.Implementations;

/// <summary>
/// Order domain service implementation
/// </summary>
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Calculates order total based on order items
    /// </summary>
    public async Task<decimal> CalculateOrderTotalAsync(List<CreateOrderItemDto> orderItems)
    {
        decimal total = 0;

        foreach (var item in orderItems)
        {
            var product = await _unitOfWork.Products.FindByIdAsync(item.ProductId);
            if (product != null)
            {
                total += product.Price * item.Quantity;
            }
        }

        return total;
    }

    /// <summary>
    /// Validates if an order can be modified
    /// </summary>
    public async Task<bool> CanModifyOrderAsync(int orderId)
    {
        var order = await _unitOfWork.Orders.FindByIdAsync(orderId);
        if (order == null) return false;

        // Business rule: Orders can only be modified within 24 hours of creation
        var timeSinceCreation = DateTime.UtcNow - order.CreatedAt;
        return timeSinceCreation.TotalHours <= 24;
    }

    /// <summary>
    /// Gets order statistics for a date range
    /// </summary>
    public async Task<OrderStatisticsDto> GetOrderStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        var orders = await _unitOfWork.Orders.FindByDateRangeAsync(startDate, endDate);
        var ordersList = orders.ToList();

        if (!ordersList.Any())
        {
            return new OrderStatisticsDto
            {
                TotalOrders = 0,
                TotalRevenue = 0,
                AverageOrderValue = 0,
                TotalItems = 0,
                TopSellingProduct = null
            };
        }

        var totalRevenue = ordersList.Sum(o => o.TotalPrice);
        var totalOrders = ordersList.Count;
        var averageOrderValue = totalRevenue / totalOrders;
        var totalItems = ordersList.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity);

        // Find top selling product
        var productSales = ordersList
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => oi.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                QuantitySold = g.Sum(oi => oi.Quantity),
                Revenue = g.Sum(oi => oi.TotalPrice)
            })
            .OrderByDescending(p => p.QuantitySold)
            .FirstOrDefault();

        ProductSalesDto? topSellingProduct = null;
        if (productSales != null)
        {
            var product = await _unitOfWork.Products.FindByIdAsync(productSales.ProductId);
            topSellingProduct = new ProductSalesDto
            {
                ProductId = productSales.ProductId,
                ProductName = product?.Name ?? "Unknown",
                QuantitySold = productSales.QuantitySold,
                Revenue = productSales.Revenue
            };
        }

        return new OrderStatisticsDto
        {
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            AverageOrderValue = averageOrderValue,
            TotalItems = totalItems,
            TopSellingProduct = topSellingProduct
        };
    }

    /// <summary>
    /// Validates order business rules
    /// </summary>
    public async Task<ValidationResult> ValidateOrderDataAsync(CreateOrderDto orderDto)
    {
        var result = new ValidationResult { IsValid = true };

        // Business rule: Customer must exist
        var customer = await _unitOfWork.Customers.FindByIdAsync(orderDto.CustomerId);
        if (customer == null)
        {
            result.IsValid = false;
            result.Errors.Add($"Customer with ID {orderDto.CustomerId} does not exist");
        }

        // Business rule: All products must exist
        foreach (var item in orderDto.OrderItems)
        {
            var product = await _unitOfWork.Products.FindByIdAsync(item.ProductId);
            if (product == null)
            {
                result.IsValid = false;
                result.Errors.Add($"Product with ID {item.ProductId} does not exist");
            }
        }

        // Business rule: Order total cannot exceed 50,000
        var total = await CalculateOrderTotalAsync(orderDto.OrderItems);
        if (total > 50000)
        {
            result.IsValid = false;
            result.Errors.Add("Order total cannot exceed €50,000");
        }

        return result;
    }

    /// <summary>
    /// Processes order items and sets unit prices from products
    /// </summary>
    public async Task<List<OrderItemDto>> ProcessOrderItemsAsync(List<CreateOrderItemDto> orderItems)
    {
        var processedItems = new List<OrderItemDto>();

        foreach (var item in orderItems)
        {
            var product = await _unitOfWork.Products.FindByIdAsync(item.ProductId);
            if (product != null)
            {
                processedItems.Add(new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }
        }

        return processedItems;
    }
}