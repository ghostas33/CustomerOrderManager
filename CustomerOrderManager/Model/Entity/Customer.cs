using System.ComponentModel.DataAnnotations;

namespace CustomerOrderManager.Models.Entities;

public class Customer
{
    public int Id { get; set; }

    [Required] [MaxLength(50)] public string FirstName { get; set; } = string.Empty;

    [Required] [MaxLength(50)] public string LastName { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string Address { get; set; } = string.Empty;

    [Required] [MaxLength(20)] public string PostalCode { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow; // ? it can be null
    
    // navigation property
    // virtual for lazy loading and also allowing Entity Framework/orm to create dynamically proxies objects
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

}