using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class OrderItemService
    {
        public async Task<OrderItem> UpdateOrderItem(OrderItem orderItem)
        {
            _context.Entry(orderItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return orderItem;
        }
    }
}