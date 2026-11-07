using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerService
    {
        public async Task<Customer> AddCustomer(Customer customer)
        {

            customer.CreatedAt = DateTime.Now; 
            
            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("customers");

            return customer;
        }
    }
}