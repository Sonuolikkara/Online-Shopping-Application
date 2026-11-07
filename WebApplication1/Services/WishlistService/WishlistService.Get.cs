using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class WishlistService
    {
        public async Task<IEnumerable<WishlistItem>>
            GetWishlistByCustomerId(int customerId)
        {
            const string sql = """
                SELECT
                    w.WishlistItemID,
                    w.CustomerID,
                    w.ProductID,
                    p.ProductName,
                    p.Price,
                    p.Stock
                FROM WishlistItems w
                INNER JOIN Products p
                    ON w.ProductID = p.ProductID
                WHERE w.CustomerID = @CustomerID
                ORDER BY w.WishlistItemID;
                """;

            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QueryAsync<WishlistItem>(
                sql,
                new
                {
                    CustomerID = customerId
                });
        }

        public async Task<bool>
            IsProductInWishlist(
                int customerId,
                int productId)
        {
            const string sql = """
                SELECT COUNT(1)
                FROM WishlistItems
                WHERE CustomerID = @CustomerID
                  AND ProductID = @ProductID;
                """;

            using var connection =
                new SqlConnection(_connectionString);

            var count =
                await connection.ExecuteScalarAsync<int>(
                    sql,
                    new
                    {
                        CustomerID = customerId,
                        ProductID = productId
                    });

            return count > 0;
        }
    }
}