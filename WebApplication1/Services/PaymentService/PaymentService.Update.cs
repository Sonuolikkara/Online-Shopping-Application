using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class PaymentService
    {
        public async Task<Payment> UpdatePayment(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return payment;
        }
    }
}