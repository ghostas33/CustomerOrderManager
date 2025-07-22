using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Service.Interfaces;

namespace CustomerOrderManager.Service.Interfaces;

/// <summary>
/// Interface for order domain service
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Calculates order total based on order items
    /// </summary>
    /// <param name="orderItems">List of order items</param>
    /// <returns>Total order amount</returns>
    Task<decimal> CalculateOrderTotalAsync(List<CreateOrderItemDto> orderItems);

    /// <summary>
    /// Validates if an order can be modified
    /// </summary>
    /// <param name="orderId">Order ID to validate</param>
    /// <returns>True if order can be modified, false otherwise</returns>
    Task<bool> CanModifyOrderAsync(int orderId);

    /// <summary>
    /// Gets order statistics for a date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Order statistics</returns>
    Task<OrderStatisticsDto> GetOrderStatisticsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Validates order business rules
    /// </summary>
    /// <param name="orderDto">Order data to validate</param>
    /// <returns>Validation result</returns>
    Task<ValidationResult> ValidateOrderDataAsync(CreateOrderDto orderDto);

    /// <summary>
    /// Processes order items and sets unit prices from products
    /// </summary>
    /// <param name="orderItems">Order items to process</param>
    /// <returns>Processed order items with unit prices</returns>
    Task<List<OrderItemDto>> ProcessOrderItemsAsync(List<CreateOrderItemDto> orderItems);
}