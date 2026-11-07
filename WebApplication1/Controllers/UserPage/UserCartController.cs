using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.UserPage
{
    [Route("Cart")]
    public class UserCartController : Controller
    {
        // GET: /Cart
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Cart/Checkout
        [HttpGet("Checkout")]
        public IActionResult Checkout(
            int? productId,
            int? quantity)
        {
            return View();
        }

        // GET: /Cart/OrderPlaced/5
        [HttpGet("OrderPlaced/{id:int}")]
        public IActionResult OrderPlaced(int id)
        {
            return View();
        }
    }
}