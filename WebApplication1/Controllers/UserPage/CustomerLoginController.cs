using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.UserPage
{
    //Returning Razor Views
    [Route("CustomerLogin")]
    public class CustomerLoginController : Controller
    {
        //Login View
        // GET: /CustomerLogin
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // REGISTER View
        // GET: /CustomerLogin/Register

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}