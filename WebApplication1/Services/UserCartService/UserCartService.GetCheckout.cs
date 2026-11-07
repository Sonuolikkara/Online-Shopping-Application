using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class UserCartService
    {
        public async Task<CheckoutViewModel> GetCheckout(
            int customerId,
            int? productId,
            int? quantity)
        {
            // ========================================================
            // BUY NOW
            // ========================================================

            if (productId.HasValue)
            {
                if (!quantity.HasValue ||
                    quantity.Value <= 0)
                {
                    quantity = 1;
                }

                var product =
                    await _productService
                        .GetProductById(
                            productId.Value);

                if (product == null)
                {
                    throw new KeyNotFoundException(
                        "Product not found.");
                }

                if (product.Stock < quantity.Value)
                {
                    throw new InvalidOperationException(
                        "Requested quantity is not available.");
                }

                return new CheckoutViewModel
                {
                    ProductId =
                        product.ProductID,

                    Quantity =
                        quantity.Value
                };
            }

            // ========================================================
            // NORMAL CART CHECKOUT
            // ========================================================

            var cartItems =
                await _cartItemService
                    .GetCartItemsByCustomerId(
                        customerId);

            if (!cartItems.Any())
            {
                throw new InvalidOperationException(
                    "Your cart is empty.");
            }

            return new CheckoutViewModel();
        }
    }
}