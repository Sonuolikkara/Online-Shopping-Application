using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class UserCartService
    {
        public async Task<bool> Remove(
            int? customerId,
            int cartItemId)
        {
            // ========================================================
            // GUEST
            // ========================================================

            if (customerId == null)
            {
                var guestCart =
                    _guestCartService.GetCart();

                var item =
                    guestCart.FirstOrDefault(
                        x => x.ProductID == cartItemId);

                if (item == null)
                {
                    return false;
                }

                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    guestCart.Remove(item);
                }

                _guestCartService.SaveCart(
                    guestCart);

                return true;
            }

            // ========================================================
            // LOGGED-IN CUSTOMER
            // ========================================================

            return await _cartItemService
                .DecreaseCartItemQuantity(
                    cartItemId);
        }
    }
}