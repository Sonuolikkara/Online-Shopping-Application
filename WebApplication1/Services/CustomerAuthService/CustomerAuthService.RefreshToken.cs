using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerAuthService
    {
        public async Task<LoginResponseModel> RefreshToken(
            string refreshToken)
        {
            //Getting Customer Detalis
            var customer =
                await _context.Customers
                    .FirstOrDefaultAsync(
                        c => c.RefreshToken == refreshToken);

            if (customer == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid refresh token.");
            }

            if (customer.RefreshTokenExpiry == null ||
                customer.RefreshTokenExpiry < DateTime.Now)
            {
                throw new UnauthorizedAccessException(
                    "Refresh token expired.");
            }

            var accessToken =
                GenerateAccessToken(customer);

            return new LoginResponseModel
            {
                CustomerId =
                    customer.CustomerID,

                AccessToken =
                    accessToken,

            };
        }
    }
}