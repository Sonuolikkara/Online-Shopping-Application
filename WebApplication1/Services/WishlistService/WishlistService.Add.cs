using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class WishlistService
    {
        public async Task<WishlistItem>
            AddToWishlist(
                int customerId,
                int productId)
        {
            const string sql = """
                INSERT INTO WishlistItems
                    (CustomerID, ProductID)
                VALUES
                    (@CustomerID, @ProductID);

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
                WHERE w.WishlistItemID =
                    CAST(SCOPE_IDENTITY() AS INT);
                """;

            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QuerySingleAsync<WishlistItem>(
                sql,
                new
                {
                    CustomerID = customerId,
                    ProductID = productId
                });
        }

        public async Task<bool> AddToWishlistIfNotExists(
            int customerId,
            int productId)
        {
            var alreadyExists =
                await IsProductInWishlist(
                    customerId,
                    productId);

            if (alreadyExists)
            {
                return false;
            }

            await AddToWishlist(
                customerId,
                productId);

            return true;
        }
    }
}