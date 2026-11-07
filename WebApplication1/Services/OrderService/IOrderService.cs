using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders();

        Task<Order?> GetOrderById(int id);

        Task<Order> AddOrder(Order order);

        Task<Order> UpdateOrder(Order order);

        Task<bool> DeleteOrder(int id);
    }
}