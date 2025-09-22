using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(e => e.StockQuantity)
                .IsRequired();
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();
        });
    }
}