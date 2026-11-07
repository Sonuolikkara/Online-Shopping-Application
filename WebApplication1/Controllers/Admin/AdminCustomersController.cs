using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminCustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public AdminCustomersController(
            ICustomerService customerService)
        {
            _customerService = customerService;
        }


        // =====================================================
        // VIEW CUSTOMERS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index(string? mode)
        {
            var customers = await _customerService.GetCustomers();

            ViewBag.Mode = mode;

            return View(customers);
        }


        // =====================================================
        // ADD CUSTOMER - GET
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // =====================================================
        // ADD CUSTOMER - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            await _customerService.AddCustomer(customer);

            return RedirectToAction(
                nameof(Index),
                new { mode = "view" });
        }


        // =====================================================
        // UPDATE CUSTOMER - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        // =====================================================
        // UPDATE CUSTOMER - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var existingCustomer =
                await _customerService.GetCustomerById(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            await _customerService.UpdateCustomer(customer);

            return RedirectToAction(
                nameof(Index),
                new { mode = "view" });
        }


        // =====================================================
        // DELETE CUSTOMER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _customerService.DeleteCustomer(id);

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