using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class OrderItemService
    {
        public async Task<OrderItem> AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);

            await _context.SaveChangesAsync();

            return orderItem;
        }
    }
}