using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CartItemService
    {
        public async Task<int?> GetCartIdByCustomerId(
            int customerId)
        {
            const string sql = """
                SELECT CartID
                FROM ShoppingCart
                WHERE CustomerID = @CustomerID;
                """;

            //Creates a .NET object representing a connection to SQL Server.
            using var connection =
                new SqlConnection(_connectionString);

            //Executes the SQL query and returns the first CartID found for the given CustomerID.
            return await connection.QueryFirstOrDefaultAsync<int?>(
                sql,
                new
                {
                    CustomerID = customerId
                });
        }


        public async Task<CartItem?> GetCartItemByProductId(
            int cartId,
            int productId)
        {
            const string sql = """
                SELECT
                    CartItemID,
                    CartID,
                    ProductID,
                    Quantity
                FROM CartItems
                WHERE CartID = @CartID
                  AND ProductID = @ProductID;
                """;

            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<CartItem?>(
                sql,
                new
                {
                    CartID = cartId,
                    ProductID = productId
                });
        }

        public async Task<bool> DecreaseCartItemQuantity(int cartItemId)
        {
            const string sql = """
                UPDATE CartItems
                SET Quantity = Quantity - 1
                WHERE CartItemID = @CartItemID
                  AND Quantity > 1;

                IF @@ROWCOUNT = 0
                BEGIN
                    DELETE FROM CartItems
                    WHERE CartItemID = @CartItemID
                      AND Quantity = 1;
                END
                """;

            using var connection =
                new SqlConnection(_connectionString);

            var rowsAffected =
                await connection.ExecuteAsync(
                    sql,
                    new
                    {
                        CartItemID = cartItemId
                    });

            return rowsAffected > 0;
        }

        public async Task<int> GetCartItemCountByCustomerId(
            int customerId)
        {
            const string sql = """
                SELECT ISNULL(SUM(ci.Quantity), 0)
                FROM CartItems ci
                INNER JOIN ShoppingCart sc
                    ON ci.CartID = sc.CartID
                WHERE sc.CustomerID = @CustomerID;
                """;

            using var connection =
                new SqlConnection(_connectionString);

            return await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    CustomerID = customerId
                });
        }


    }

}