using CustomerOrderManager.Data.DbContext;
using CustomerOrderManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderManager.Repository;

public interface ICustomerRepository
{
    Task<Customer?> FindByIdAsync(int id);
    Task<IEnumerable<Customer>> FindAllAsync();
    Task<Customer> SaveAsync(Customer customer);
    Task DeleteByIdAsync(int id);
    Task<Customer?> FindByNameAsync(string firstName, string lastName);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    
    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> FindByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Customer>> FindAllAsync()
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .ToListAsync();
    }

    public async Task<Customer> SaveAsync(Customer customer)
    {
        if (customer.Id == 0)
        {
            customer.CreatedAt = DateTime.UtcNow;
            _context.Customers.Add(customer);
        }
        else
        {
            customer.UpdatedAt = DateTime.UtcNow;
            _context.Customers.Update(customer);
        }

        return customer;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var customer = await FindByIdAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
        }
    }

    public async Task<Customer?> FindByNameAsync(string firstName, string lastName)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.FirstName == firstName && c.LastName == lastName);
    }
    
}