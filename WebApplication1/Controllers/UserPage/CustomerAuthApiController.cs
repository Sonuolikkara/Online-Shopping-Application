using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.UserPage
{
    // /api/CustomerAuthApi
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAuthApiController : ControllerBase
    {
        private readonly ICustomerAuthService _customerAuthService;

        public CustomerAuthApiController(
            ICustomerAuthService customerAuthService)
        {
            _customerAuthService = customerAuthService;
        }

        // LOGIN
        // POST: /api/CustomerAuthApi/login

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            CustomerLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);//400 Bad Request
            }

            try
            {
                // Login + both token generation
                var loginResponse =
                    await _customerAuthService.Login(model);


                // Merge Guest Cart into Customer Cart
                await _customerAuthService
                    .MergeGuestCartIntoCustomerCart(
                        loginResponse.CustomerId);

                return Ok(new
                {
                    message = "Login successful.",

                    customerId =
                        loginResponse.CustomerId,

                    accessToken =
                        loginResponse.AccessToken
                });//200
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message //401 Unauthorized
                });
            }
        }


        // REGISTER
        // POST: /api/CustomerAuthApi/register

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            CustomerRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);//400 Bad Request
            }

            try
            {
                //Actual Registration Workflow
                await _customerAuthService.Register(model);



                return Ok(new
                {
                    message =
                        "Registration successful. Please login.",

                });//200
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });//401
            }
        }

        // REFRESH ACCESS TOKEN
        // POST: /api/CustomerAuthApi/refresh

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            //Read refresh token from cookie
            var refreshToken =
                Request.Cookies["customerRefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new
                {
                    message = "Refresh token missing."
                });
            }

            try
            {
                var response =
                    await _customerAuthService
                        .RefreshToken(refreshToken);

                return Ok(new
                {
                    customerId =
                        response.CustomerId,

                    accessToken =
                        response.AccessToken
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }
        // LOGOUT
        // POST: /api/CustomerAuthApi/logout

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {

            var refreshToken =
                Request.Cookies["customerRefreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _customerAuthService
                    .Logout(refreshToken);
            }

            Response.Cookies.Delete(
                "customerRefreshToken");
            return Ok(new
            {
                message = "Logout successful."
            });
        }
    }
}