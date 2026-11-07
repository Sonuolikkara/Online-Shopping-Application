using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int? CategoryID { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    public class Customer
    {
        public int CustomerID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string? PasswordHash { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }
    }

    public class ShoppingCart
    {
        public int CartID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CartItem
    {
        public int CartItemID { get; set; }
        public int? CartID { get; set; }
        public int? ProductID { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int? OrderID { get; set; }
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }

    public class Payment
    {
        public int PaymentID { get; set; }
        public int? OrderID { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class WishlistItem
    {
        public int WishlistItemID { get; set; }

        public int CustomerID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}
