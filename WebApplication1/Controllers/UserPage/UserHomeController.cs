using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.UserPage
{
    [Route("UserHome")]
    public class UserHomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}