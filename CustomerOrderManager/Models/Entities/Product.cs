using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerOrderManager.Models.Entities;

public class Product
{
    public int Id { get; set; }
    
    [Required] 
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // navigation property
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
}