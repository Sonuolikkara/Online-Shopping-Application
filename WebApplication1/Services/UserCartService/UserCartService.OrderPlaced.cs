using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class UserCartService
    {
        public async Task<Order?> GetOrderPlaced(
            int orderId)
        {
            var order =
                await _orderService
                    .GetOrderById(orderId);

            return order;
        }
    }
}