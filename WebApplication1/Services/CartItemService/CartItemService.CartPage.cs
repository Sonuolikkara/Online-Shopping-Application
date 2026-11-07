using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CartItemService
    {
        public async Task<IEnumerable<CartItemViewModel>>
            GetCartItemsByCustomerId(int customerId)
        {
            const string sql = """
                SELECT
                    ci.CartItemID,
                    ci.ProductID,
                    p.ProductName,
                    p.Price,
                    ci.Quantity,
                    p.Stock,
                    p.ImageUrl
                FROM CartItems ci
                INNER JOIN ShoppingCart sc
                    ON ci.CartID = sc.CartID
                INNER JOIN Products p
                    ON ci.ProductID = p.ProductID
                WHERE sc.CustomerID = @CustomerID
                ORDER BY ci.CartItemID;
                """;

            using var connection =
                new SqlConnection(_connectionString);

            var cartItems =
                await connection.QueryAsync<CartItemViewModel>(
                    sql,
                    new
                    {
                        CustomerID = customerId
                    });

            return cartItems;
        }
    }
}