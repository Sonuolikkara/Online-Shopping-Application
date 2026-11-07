using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.UserPage
{
    [Route("Wishlist")]
    public class UserWishlistController : Controller
    {
        // WISHLIST PAGE
        // GET: /Wishlist

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}