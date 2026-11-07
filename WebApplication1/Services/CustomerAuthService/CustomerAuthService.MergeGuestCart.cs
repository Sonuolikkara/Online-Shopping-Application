using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerAuthService
    {
        public async Task MergeGuestCartIntoCustomerCart(
            int customerId)
        {
            //Get the guest cart
            var guestCart =
                _guestCartService.GetCart();

            if (guestCart.Count == 0)
            {
                return;
            }

            //Find customer's existing cart
            var cartId =
                await _cartItemService
                    .GetCartIdByCustomerId(
                        customerId);

            if (cartId == null)
            {
                var cart = new ShoppingCart
                {
                    CustomerID = customerId,
                    CreatedDate = DateTime.Now
                };

                _context.ShoppingCart.Add(cart);

                await _context.SaveChangesAsync();

                cartId = cart.CartID;
            }

            foreach (var guestItem in guestCart)
            {
                var product =
                    await _productService
                        .GetProductById(
                            guestItem.ProductID);

                if (product == null ||
                    product.Stock <= 0)
                {
                    continue;
                }

                var existingCartItem =
                    await _cartItemService
                        .GetCartItemByProductId(
                            cartId.Value,
                            guestItem.ProductID);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity =
                        Math.Min(
                            existingCartItem.Quantity +
                            guestItem.Quantity,
                            product.Stock);

                    await _cartItemService
                        .UpdateCartItem(
                            existingCartItem);
                }
                else
                {
                    var quantity =
                        Math.Min(
                            guestItem.Quantity,
                            product.Stock);

                    if (quantity <= 0)
                    {
                        continue;
                    }

                    //Create a New CartItem
                    await _cartItemService
                        .AddCartItem(
                            new CartItem
                            {
                                CartID =
                                    cartId.Value,

                                ProductID =
                                    guestItem.ProductID,

                                Quantity =
                                    quantity
                            });
                }
            }

            //Clear the guest cart
            _guestCartService.ClearCart();
        }
    }
}