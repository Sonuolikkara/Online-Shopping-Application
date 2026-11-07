using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class OrderService
    {
        public async Task<Order> UpdateOrder(Order order)
        {
            var existingOrder = await _context.Orders
                .FindAsync(order.OrderID);

            if (existingOrder == null)
            {
                throw new Exception("Order not found.");
            }

            existingOrder.CustomerID = order.CustomerID;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.Status = order.Status;

            await _context.SaveChangesAsync();

            return existingOrder;
        }
    }
}