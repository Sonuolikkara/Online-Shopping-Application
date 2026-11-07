using Dapper;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Services
{
    public partial class CartItemService
    {
        //Clearing Cart Item after Normal Checkout
        public async Task ClearCartByCustomerId(int customerId)
        {
            const string sql = """
                DELETE FROM CartItems
                WHERE CartID IN
                (
                    SELECT CartID
                    FROM ShoppingCart
                    WHERE CustomerID = @CustomerID
                );
                """;

            using var connection =
                new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                sql,
                new
                {
                    CustomerID = customerId
                });
        }
    }
}