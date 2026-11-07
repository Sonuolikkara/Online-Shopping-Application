using Dapper;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Services
{
    public partial class WishlistService
    {
        public async Task<bool>
            RemoveFromWishlist(
                int customerId,
                int productId)
        {
            const string sql = """
                DELETE FROM WishlistItems
                WHERE CustomerID = @CustomerID
                  AND ProductID = @ProductID;
                """;

            using var connection =
                new SqlConnection(_connectionString);

            var rowsAffected =
                await connection.ExecuteAsync(
                    sql,
                    new
                    {
                        CustomerID = customerId,
                        ProductID = productId
                    });

            return rowsAffected > 0;
        }
    }
}