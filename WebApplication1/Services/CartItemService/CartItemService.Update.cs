using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CartItemService
    {
        public async Task<CartItem> UpdateCartItem(CartItem cartItem)
        {
            const string sql = """
                UPDATE CartItems
                SET CartID = @CartID,
                    ProductID = @ProductID,
                    Quantity = @Quantity
                WHERE CartItemID = @CartItemID;
                """;

            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                sql,
                new
                {
                    cartItem.CartItemID,
                    cartItem.CartID,
                    cartItem.ProductID,
                    cartItem.Quantity
                });

            return cartItem;
        }
    }
}