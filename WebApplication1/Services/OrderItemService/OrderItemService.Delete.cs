using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class OrderItemService
    {
        public async Task<bool> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
                return false;

            _context.OrderItems.Remove(orderItem);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}