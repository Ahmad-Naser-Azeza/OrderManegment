using Microsoft.EntityFrameworkCore;

namespace Order.Infrastructure;

public sealed class OrderDbContext : DbContext
{
    public OrderDbContext() { } 
    public OrderDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
     }
}