using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class PaymentService
    {
        public async Task<Payment> AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            return payment;
        }
    }
}