namespace WebApplication1.Models
{
    public class CartItemViewModel
    {
        public int CartItemID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public decimal Total => Price * Quantity;
    }
}