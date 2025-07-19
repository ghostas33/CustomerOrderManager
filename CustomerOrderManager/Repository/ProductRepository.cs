using CustomerOrderManager.Data.DbContext;
using CustomerOrderManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderManager.Repository;

public interface IProductRepository
{
    Task<Product?> FindByIdAsync(int id);
    Task<IEnumerable<Product>> FindAllAsync();
    Task<Product> SaveAsync(Product product);
    Task DeleteByIdAsync(int id);
    Task<IEnumerable<Product>> FindByNameContainingAsync(string name);
}

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> FindByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> FindAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> SaveAsync(Product product)
    {
        if (product.Id == 0)
        {
            product.CreatedAt = DateTime.UtcNow;
            _context.Products.Add(product);
        }
        else
        {
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
        }
        return product;
    }
    
    public async Task DeleteByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
    }
    
    public async Task<IEnumerable<Product>> FindByNameContainingAsync(string name)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(name))
            .ToListAsync();
    }
}