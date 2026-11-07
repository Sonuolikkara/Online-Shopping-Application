using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ICustomerAuthService
    {
        Task Register(
            CustomerRegisterModel model);

        Task<LoginResponseModel> Login(
            CustomerLoginModel model);

        Task MergeGuestCartIntoCustomerCart(int customerId);

        Task<LoginResponseModel> RefreshToken(
            string refreshToken);

        Task Logout(
            string refreshToken);
    }
}