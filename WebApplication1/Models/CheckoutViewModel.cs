using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter the correct phone number.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a payment method.")]
        public string PaymentMethod { get; set; } = string.Empty;

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }
    }
}