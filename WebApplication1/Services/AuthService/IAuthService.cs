using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IAuthService
    {
        LoginResponseModel Login(LoginModel model);

        LoginResponseModel RefreshToken(RefreshTokenRequest request);

    }
}
