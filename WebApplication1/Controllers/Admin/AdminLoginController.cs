using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.Admin
{
    [Route("AdminLogin")]
    public class AdminLoginController : Controller
    {
        private readonly IAuthService _authService;

        public AdminLoginController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /AdminLogin
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /AdminLogin
        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _authService.Login(model);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(
                    claims,
                    "AdminCookie");

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    "AdminCookie",
                    principal);

                return Redirect("/Admin/Dashboard");
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid username or password.");

                return View(model);
            }
        }

        // POST: /AdminLogin/Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminCookie");

            return RedirectToAction(nameof(Index));
        }
    }
}