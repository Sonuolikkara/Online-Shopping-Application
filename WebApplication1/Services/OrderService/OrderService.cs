using WebApplication1.Data;

namespace WebApplication1.Services
{
    public partial class OrderService : IOrderService
    {
        private readonly ShoppingCartDbContext _context;

        public OrderService(ShoppingCartDbContext context)
        {
            _context = context;
        }
    }
}