using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerService
    {
        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var existingCustomer = await _context.Customers
                .FindAsync(customer.CustomerID);

            if (existingCustomer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;

            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("customers");

            return existingCustomer;
        }
    }
}