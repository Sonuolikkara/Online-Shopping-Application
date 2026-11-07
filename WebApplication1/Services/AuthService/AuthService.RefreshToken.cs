using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class AuthService
    {
        public LoginResponseModel RefreshToken(
            RefreshTokenRequest request)
        {
            var admin =
                _context.Admins.FirstOrDefault(a =>
                    a.RefreshToken ==
                    request.RefreshToken);

            if (admin == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid refresh token");
            }

            if (admin.RefreshTokenExpiry <
                DateTime.Now)
            {
                throw new UnauthorizedAccessException(
                    "Refresh token expired");
            }

            var jwt =
                GenerateAccessToken(
                    admin.Username);

            return new LoginResponseModel
            {
                AccessToken = jwt,
                RefreshToken =
                    admin.RefreshToken!
            };
        }
    }
}