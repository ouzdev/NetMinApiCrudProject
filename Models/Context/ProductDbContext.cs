using Microsoft.EntityFrameworkCore;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions context) : base(context)
    {
    }
    public DbSet<Product> Products { get; set; }
}

