using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using WebApplication1.Data;
using WebApplication1.Models;


namespace WebApplication1.Services
{
    public partial class CustomerService : ICustomerService
    {
        private readonly ShoppingCartDbContext _context;

        private readonly IDistributedCache _cache;

        private readonly string _connectionString;

        public CustomerService(
            ShoppingCartDbContext context,
            IDistributedCache cache,
            IConfiguration configuration)
        {
            _context = context;
            _cache = cache;

            _connectionString =
                configuration.GetConnectionString("ShoppingCartConnection")!;
        }
    }
}
