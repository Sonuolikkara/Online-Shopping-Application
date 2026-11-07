using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Data;

namespace WebApplication1.Services
{
    public partial class PaymentService : IPaymentService
    {
        private readonly ShoppingCartDbContext _context;

        public PaymentService(ShoppingCartDbContext context)
        {
            _context = context;
        }
    }
}