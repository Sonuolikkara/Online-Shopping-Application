using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IUserCartService
    {
        Task<List<CartItemViewModel>> GetCart(
            int? customerId);

        Task<bool> Remove(
            int? customerId,
            int cartItemId);

        Task<CheckoutViewModel> GetCheckout(
            int customerId,
            int? productId,
            int? quantity);

        Task<int> Checkout(
            int customerId,
            CheckoutViewModel model);

        Task<Order?> GetOrderPlaced(
            int orderId);
    }
}