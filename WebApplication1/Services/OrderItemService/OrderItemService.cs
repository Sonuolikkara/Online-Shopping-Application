using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Data;

namespace WebApplication1.Services
{
    public partial class OrderItemService : IOrderItemService
    {
        private readonly ShoppingCartDbContext _context;

        public OrderItemService(ShoppingCartDbContext context)
        {
            _context = context;
        }
    }
}