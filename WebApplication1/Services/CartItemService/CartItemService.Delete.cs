using Dapper;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Services
{
    public partial class CartItemService
    {
        public async Task<bool> DeleteCartItem(int id)
        {
            const string sql = """
                DELETE FROM CartItems
                WHERE CartItemID = @CartItemID;
                """;

            using var connection = new SqlConnection(_connectionString);

            var rowsAffected = await connection.ExecuteAsync(
                sql,
                new { CartItemID = id }
            );

            return rowsAffected > 0;
        }
    }
}