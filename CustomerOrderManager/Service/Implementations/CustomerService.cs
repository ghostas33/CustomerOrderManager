using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Repository;
using CustomerOrderManager.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using ValidationResult = CustomerOrderManager.Service.Interfaces.ValidationResult;
using CustomerStatisticsDto = CustomerOrderManager.Service.Interfaces.CustomerStatisticsDto;

namespace CustomerOrderManager.Service.Implementations;

/// <summary>
/// Customer domain service implementation
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Validates if a customer can be deleted
    /// </summary>
    public async Task<bool> CanDeleteCustomerAsync(int customerId)
    {
        // Business rule: Customer cannot be deleted if they have orders
        var orders = await _unitOfWork.Orders.FindByCustomerIdAsync(customerId);
        return !orders.Any();
    }

    /// <summary>
    /// Gets customer statistics
    /// </summary>
    public async Task<CustomerStatisticsDto> GetCustomerStatisticsAsync(int customerId)
    {
        var orders = await _unitOfWork.Orders.FindByCustomerIdAsync(customerId);
        var ordersList = orders.ToList();

        if (!ordersList.Any())
        {
            return new CustomerStatisticsDto
            {
                TotalOrders = 0,
                TotalSpent = 0,
                LastOrderDate = null,
                AverageOrderValue = 0
            };
        }

        var totalSpent = ordersList.Sum(o => o.TotalPrice);
        var totalOrders = ordersList.Count;
        var lastOrderDate = ordersList.Max(o => o.OrderDate);
        var averageOrderValue = totalSpent / totalOrders;

        return new CustomerStatisticsDto
        {
            TotalOrders = totalOrders,
            TotalSpent = totalSpent,
            LastOrderDate = lastOrderDate,
            AverageOrderValue = averageOrderValue
        };
    }

    /// <summary>
    /// Validates customer data for business rules
    /// </summary>
    public async Task<ValidationResult> ValidateCustomerDataAsync(CreateCustomerDto customerDto)
    {
        var result = new ValidationResult { IsValid = true };

        // Business rule: Check for duplicate customers (same first name, last name, and address)
        var existingCustomer = await _unitOfWork.Customers.FindByNameAsync(customerDto.FirstName, customerDto.LastName);
        if (existingCustomer != null && existingCustomer.Address.Equals(customerDto.Address, StringComparison.OrdinalIgnoreCase))
        {
            result.IsValid = false;
            result.Errors.Add("A customer with the same name and address already exists");
        }

        // Business rule: Postal code format validation (Greek postal codes)
        if (!IsValidGreekPostalCode(customerDto.PostalCode))
        {
            result.IsValid = false;
            result.Errors.Add("Invalid Greek postal code format. Must be 5 digits starting with 1-9");
        }

        return result;
    }

    /// <summary>
    /// Validates Greek postal code format
    /// </summary>
    private static bool IsValidGreekPostalCode(string postalCode)
    {
        // Greek postal codes are 5 digits, first digit 1-9, remaining digits 0-9
        return postalCode.Length == 5 && 
               char.IsDigit(postalCode[0]) && 
               postalCode[0] != '0' && 
               postalCode.Skip(1).All(char.IsDigit);
    }
}