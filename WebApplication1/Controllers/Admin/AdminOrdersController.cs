using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminOrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public AdminOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        // =====================================================
        // VIEW ORDERS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index(string? mode)
        {
            var orders = await _orderService.GetOrders();

            ViewBag.Mode = mode;

            return View(orders);
        }


        // =====================================================
        // ADD ORDER - GET
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // =====================================================
        // ADD ORDER - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            await _orderService.AddOrder(order);

            return RedirectToAction(
                nameof(Index),
                new { mode = "view" });
        }


        // =====================================================
        // UPDATE ORDER - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // =====================================================
        // UPDATE ORDER - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Order order)
        {
            if (id != order.OrderID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(order);
            }

            var existingOrder =
                await _orderService.GetOrderById(id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderService.UpdateOrder(order);

            return RedirectToAction(
                nameof(Index),
                new { mode = "view" });
        }


        // =====================================================
        // DELETE ORDER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _orderService.DeleteOrder(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(
                nameof(Index),
                new { mode = "view" });
        }
    }
}