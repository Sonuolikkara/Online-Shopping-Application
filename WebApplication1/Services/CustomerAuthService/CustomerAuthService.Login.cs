using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerAuthService
    {
        
        public async Task<LoginResponseModel> Login(
            CustomerLoginModel model)
        {
            // Find customer using email
            var customer =
                await _context.Customers
                    .FirstOrDefaultAsync(
                        c => c.Email == model.Email);

            if (customer == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            // Check whether password exists
            if (string.IsNullOrEmpty(
                    customer.PasswordHash))
            {
                throw new UnauthorizedAccessException(
                    "This account is not registered for customer login.");
            }

            // Verify password
            var result =
                _passwordHasher.VerifyHashedPassword(
                    customer,
                    customer.PasswordHash,
                    model.Password);

            if (result ==
                PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var accessToken = GenerateAccessToken(customer);

            var refreshToken = GenerateRefreshToken();

            customer.RefreshToken =
                refreshToken;

            customer.RefreshTokenExpiry =
                DateTime.Now.AddDays(30);

            //Saving to Customer Table
            await _context.SaveChangesAsync();


            // Store refresh token in HttpOnly cookie
            _httpContextAccessor.HttpContext?
                .Response.Cookies.Append(
                    "customerRefreshToken",
                    refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,//CSRS Protection
                        Expires =
                            DateTimeOffset.Now.AddDays(30)
                    });

            return new LoginResponseModel
            {
                CustomerId =
                    customer.CustomerID,

                AccessToken =
                    accessToken
            };
        }
    }
}