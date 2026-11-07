using Hangfire;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerAuthService
    {
        // Register
        public async Task Register(
            CustomerRegisterModel model)
        {
            // Check if email already exists
            //LINQ
            var existingCustomer =
                await _context.Customers
                    .FirstOrDefaultAsync(
                        c => c.Email == model.Email);

            if (existingCustomer != null)
            {
                throw new InvalidOperationException(
                    "An account with this email already exists.");
            }

            // Create customer
            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                CreatedAt = DateTime.Now
            };

            // Hash password
            customer.PasswordHash =
                _passwordHasher.HashPassword(
                    customer,
                    model.Password);

            // Save customer
            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            // Create a new shopping cart row for the customer
            var cart = new ShoppingCart
            {
                CustomerID = customer.CustomerID,
                CreatedDate = DateTime.Now
            };

            _context.ShoppingCart.Add(cart);

            await _context.SaveChangesAsync();

            // Send welcome email through Hangfire
            BackgroundJob.Enqueue<IEmailService>(
                emailService =>
                    emailService.SendWelcomeEmail(
                        customer.Email,
                        customer.FirstName));

            return;
        }
    }
}