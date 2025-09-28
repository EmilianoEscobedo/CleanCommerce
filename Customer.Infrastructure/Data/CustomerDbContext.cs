using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Data;
using Domain.Entities;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.Street)
                    .IsRequired()
                    .HasMaxLength(200);

                address.Property(a => a.Number)
                    .IsRequired();
            });

            entity.Property(e => e.RegistrationDate)
                .IsRequired();
        });
    }
}