using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetOrderItems();

        Task<OrderItem?> GetOrderItemById(int id);

        Task<OrderItem> AddOrderItem(OrderItem orderItem);

        Task<OrderItem> UpdateOrderItem(OrderItem orderItem);

        Task<bool> DeleteOrderItem(int id);
    }
}