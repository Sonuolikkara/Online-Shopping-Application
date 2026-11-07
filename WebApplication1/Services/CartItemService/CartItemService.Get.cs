using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CartItemService
    {
        public async Task<IEnumerable<CartItem>> GetCartItems()
        {
            //dapper Sql Query
            const string sql = """
                SELECT *
                FROM CartItems
                """;

            using var connection = new SqlConnection(_connectionString);

            var cartItems = await connection.QueryAsync<CartItem>(sql);

            return cartItems;
        }

        public async Task<CartItem?> GetCartItemById(int id)
        {
            const string sql = """
                SELECT *
                FROM CartItems
                WHERE CartItemID = @Id
                """;

            using var connection = new SqlConnection(_connectionString);

            var cartItem = await connection.QueryFirstOrDefaultAsync<CartItem>(
                sql,
                new { Id = id }
            );

            return cartItem;
        }
    }
}