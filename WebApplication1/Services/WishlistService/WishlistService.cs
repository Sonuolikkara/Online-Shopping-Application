using Microsoft.Data.SqlClient;

namespace WebApplication1.Services
{
    public partial class WishlistService : IWishlistService
    {
        private readonly string _connectionString;

        public WishlistService(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString(
                    "ShoppingCartConnection")!;
        }
    }
}