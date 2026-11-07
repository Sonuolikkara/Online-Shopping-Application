using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CartItemService : ICartItemService
    {
        private readonly string _connectionString;

        public CartItemService(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("ShoppingCartConnection")!;
        }
    }
}