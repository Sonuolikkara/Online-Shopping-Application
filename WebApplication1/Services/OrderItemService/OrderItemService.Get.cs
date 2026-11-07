using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class OrderItemService
    {
        public async Task<IEnumerable<OrderItem>> GetOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemById(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }
    }
}
