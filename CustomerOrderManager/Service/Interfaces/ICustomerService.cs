using CustomerOrderManager.Model.DTO;

namespace CustomerOrderManager.Service.Interfaces;

/// <summary>
/// Interface for customer domain service
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Validates if a customer can be deleted
    /// </summary>
    /// <param name="customerId">Customer ID to validate</param>
    /// <returns>True if customer can be deleted, false otherwise</returns>
    Task<bool> CanDeleteCustomerAsync(int customerId);

    /// <summary>
    /// Gets customer statistics
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>Customer statistics</returns>
    Task<CustomerStatisticsDto> GetCustomerStatisticsAsync(int customerId);

    /// <summary>
    /// Validates customer data for business rules
    /// </summary>
    /// <param name="customerDto">Customer data to validate</param>
    /// <returns>Validation result</returns>
    Task<ValidationResult> ValidateCustomerDataAsync(CreateCustomerDto customerDto);
}

/// <summary>
/// Customer statistics DTO
/// </summary>
public class CustomerStatisticsDto
{
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastOrderDate { get; set; }
    public decimal AverageOrderValue { get; set; }
}

/// <summary>
/// Validation result
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}