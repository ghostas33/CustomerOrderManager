using CustomerOrderManager.Data.DbContext;
using CustomerOrderManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderManager.Repository;

public interface IOrderRepository
{
    Task<Order?> FindByIdAsync(int id);
    Task<IEnumerable<Order>> FindAllAsync();
    Task<Order> SaveAsync(Order order);
    Task DeleteByIdAsync(int id);
    Task<IEnumerable<Order>> FindByCustomerIdAsync(int customerId);
    Task<IEnumerable<Order>> FindByDateRangeAsync(DateTime startDate, DateTime endDate);
}

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Order?> FindByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    
    public async Task<IEnumerable<Order>> FindAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }

    public async Task<Order> SaveAsync(Order order)
    {
        if (order.Id == 0)
        {
            order.CreatedAt = DateTime.UtcNow;
            _context.Orders.Add(order);
        }
        else
        {
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(order);
        }

        return order;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
        }
    }
    
    public async Task<IEnumerable<Order>> FindByCustomerIdAsync(int customerId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Order>> FindByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderBy(o => o.OrderDate)
            .ToListAsync();
    }
}