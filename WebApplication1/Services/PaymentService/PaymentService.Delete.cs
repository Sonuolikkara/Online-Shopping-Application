using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class PaymentService
    {
        public async Task<bool> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
                return false;

            _context.Payments.Remove(payment);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}