using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CartItemService
    {
        public async Task<CartItem> AddCartItem(CartItem cartItem)
        {
            const string sql = """
                INSERT INTO CartItems (CartID, ProductID, Quantity)
                VALUES (@CartID, @ProductID, @Quantity);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            using var connection = new SqlConnection(_connectionString);

            var id = await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    cartItem.CartID,
                    cartItem.ProductID,
                    cartItem.Quantity
                });

            cartItem.CartItemID = id;

            return cartItem;
        }

        public async Task AddToCart(
                   int customerId,
                   int productId,
                   int quantity,
                   int stock)
        {
            var cartId =
                await GetCartIdByCustomerId(customerId);

            if (cartId == null)
            {
                throw new InvalidOperationException(
                    "Shopping cart not found.");
            }

            var existingCartItem =
                await GetCartItemByProductId(
                    cartId.Value,
                    productId);

            if (existingCartItem != null)
            {
                var newQuantity =
                    existingCartItem.Quantity + quantity;

                if (newQuantity > stock)
                {
                    throw new InvalidOperationException(
                        "Requested quantity exceeds available stock.");
                }

                existingCartItem.Quantity = newQuantity;

                await UpdateCartItem(existingCartItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartID = cartId.Value,
                    ProductID = productId,
                    Quantity = quantity
                };

                await AddCartItem(cartItem);
            }
        }
    }
}