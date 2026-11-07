using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class UserCartService
    {
        public async Task<List<CartItemViewModel>> GetCart(
            int? customerId)
        {
            // ========================================================
            // GUEST CART
            // ========================================================

            if (customerId == null)
            {
                var guestCart =
                    _guestCartService.GetCart();

                var guestCartItems =
                    new List<CartItemViewModel>();

                foreach (var guestItem in guestCart)
                {
                    var product =
                        await _productService
                            .GetProductById(
                                guestItem.ProductID);

                    if (product == null)
                    {
                        continue;
                    }

                    guestCartItems.Add(
                        new CartItemViewModel
                        {
                            ProductID =
                                product.ProductID,

                            ProductName =
                                product.ProductName,

                            Price =
                                product.Price,

                            Quantity =
                                guestItem.Quantity,

                            Stock =
                                product.Stock,

                            ImageUrl =
                                product.ImageUrl
                        });
                }

                return guestCartItems;
            }

            // ========================================================
            // LOGGED-IN CUSTOMER CART
            // ========================================================

            var cartItems =
                await _cartItemService
                    .GetCartItemsByCustomerId(
                        customerId.Value);

            return cartItems.ToList();
        }
    }
}