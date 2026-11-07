using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerService
    {
        public async Task<bool> DeleteCustomer(int id)
        {
            var customer = await _context.Customers
                .Where(c => c.CustomerID == id)
                .FirstOrDefaultAsync();

            if (customer == null)
                return false;

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("customers");

            return true;
        }
    }
}