using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public partial class CustomerAuthService
    {
        public async Task Logout(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.RefreshToken == refreshToken);

            if (customer == null)
            {
                return;
            }

            customer.RefreshToken = null;
            customer.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();
        }
    }
}
