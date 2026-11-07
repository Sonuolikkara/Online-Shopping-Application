using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public AdminController(
            IProductService productService,
            ICategoryService categoryService,
            ICustomerService customerService,
            IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _customerService = customerService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var products = await _productService.GetProducts();
            var categories = await _categoryService.GetCategories();
            var customers = await _customerService.GetCustomers();
            var orders = await _orderService.GetOrders();

            // Recent orders //LINQ
            var recentOrders = orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            // Low stock products //LINQ
            var lowStockProducts = products
                .Where(p => p.Stock <= 5)
                .OrderBy(p => p.Stock)
                .Take(5)
                .ToList();

            // Order status counts
            var deliveredOrders = orders.Count(o =>
                string.Equals(
                    o.Status,
                    "Delivered",
                    StringComparison.OrdinalIgnoreCase));

            var processingOrders = orders.Count(o =>
                string.Equals(
                    o.Status,
                    "Processing",
                    StringComparison.OrdinalIgnoreCase));

            var pendingOrders = orders.Count(o =>
                string.Equals(
                    o.Status,
                    "Pending",
                    StringComparison.OrdinalIgnoreCase));

            var model = new AdminDashboardViewModel
            {
                // Summary counts
                ProductCount = products.Count(),
                CategoryCount = categories.Count(),
                CustomerCount = customers.Count(),
                OrderCount = orders.Count(),

                // Order status counts
                DeliveredOrderCount = deliveredOrders,
                ProcessingOrderCount = processingOrders,
                PendingOrderCount = pendingOrders,

                // Dashboard details
                RecentOrders = recentOrders,
                LowStockProducts = lowStockProducts
            };

            return View(model);
        }
    }
}