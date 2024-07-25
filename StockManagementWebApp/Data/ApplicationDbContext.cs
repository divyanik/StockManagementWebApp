using Microsoft.EntityFrameworkCore;
using StockManagementWebApp.Models;
using System.Collections.Generic;

namespace StockManagementWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItems> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Product entity
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id); // Primary key
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Set decimal precision

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id); // Primary key
            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            // Configure Order entity
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id); // Primary key
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId); // Foreign key

            // Configure OrderItem entity
            modelBuilder.Entity<CartItems>()
                .HasKey(oi => oi.Id); // Primary key
            modelBuilder.Entity<CartItems>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.CartItems)
                .HasForeignKey(oi => oi.OrderId); // Foreign key
            modelBuilder.Entity<CartItems>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId); // Foreign key

            // Configure the decimal property
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // or use .HasPrecision(18, 2)

       // Optional: Configure table names if they differ from DbSet names
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<CartItems>().ToTable("CartItems");
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}