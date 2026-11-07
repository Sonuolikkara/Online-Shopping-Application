using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.UserPage
{
    [Route("UserProducts")]
    public class UserProductsController : Controller
    {
        // GET: /UserProducts
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /UserProducts/Details/5
        [HttpGet("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}