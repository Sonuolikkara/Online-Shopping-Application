using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class AuthService
    {
        //Login Validation
        public LoginResponseModel Login(
            LoginModel model)
        {
            //Find the admin by username and password
            var admin =
                _context.Admins.FirstOrDefault(a =>
                    a.Username == model.Username &&
                    a.Password == model.Password);

            if (admin == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid username or password");
            }

            //Refresh token generation
            var refreshToken =
                GenerateRefreshToken();

            admin.RefreshToken =
                refreshToken;

            admin.RefreshTokenExpiry =
                DateTime.Now.AddDays(30);

            _context.SaveChanges();

            var jwt =
                GenerateAccessToken(
                    model.Username);

            return new LoginResponseModel
            {
                AccessToken = jwt,
                RefreshToken = refreshToken
            };
        }
    }
}