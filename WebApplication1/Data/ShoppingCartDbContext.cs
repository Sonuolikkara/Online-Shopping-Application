using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(
            DbContextOptions<ShoppingCartDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Database triggers
            modelBuilder.Entity<Product>()
                .ToTable("Products", table =>
                    table.HasTrigger("TR_Products_Delete"));

            modelBuilder.Entity<Order>()
                .ToTable("Orders", table =>
                    table.HasTrigger("TR_Orders_Delete"));

            modelBuilder.Entity<Category>()
                .ToTable("Categories", table =>
                    table.HasTrigger("TR_Categories_Delete"));

            modelBuilder.Entity<Customer>()
                .ToTable("Customers", table =>
                    table.HasTrigger("TR_Customers_Delete"));

            // Primary Keys
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductID);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryID);

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerID);

            modelBuilder.Entity<ShoppingCart>()
                .HasKey(c => c.CartID);

            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.CartItemID);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderID);

            modelBuilder.Entity<OrderItem>()
                .HasKey(o => o.OrderItemID);

            modelBuilder.Entity<Payment>()
                .HasKey(p => p.PaymentID);

            // Decimal precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);


            modelBuilder.Entity<Product>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryID);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(c => c.CustomerID);

            modelBuilder.Entity<CartItem>()
                .HasOne<ShoppingCart>()
                .WithMany()
                .HasForeignKey(c => c.CartID);

            modelBuilder.Entity<CartItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(c => c.ProductID);

            modelBuilder.Entity<Order>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerID);

            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(o => o.OrderID);

            modelBuilder.Entity<OrderItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(o => o.ProductID);

            modelBuilder.Entity<Payment>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(p => p.OrderID);
        }
    }
}
