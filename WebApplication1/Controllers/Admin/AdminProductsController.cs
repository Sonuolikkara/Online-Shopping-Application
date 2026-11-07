using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminProductsController : Controller
    {
        private readonly IProductService _productService;

        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //Get Products
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();

            return View(products);
        }

        //Get Create Form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Get Edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        //Editing the Products
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var existingProduct = await _productService.GetProductById(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.UpdateProduct(product);

            return RedirectToAction(nameof(Index));
        }

        //Creating a New Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productService.AddProduct(product);

            return RedirectToAction(nameof(Index));
        }
        //Delete a Product by id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProduct(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }


        //Multiple Dlt

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected(IEnumerable<int> productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                TempData["Error"] = "Please select at least one product.";
                return RedirectToAction(nameof(Index));
            }

            var deleted = await _productService.DeleteSelectedProducts(productIds);

            if (!deleted)
            {
                TempData["Error"] = "No selected products were found.";
            }

            return RedirectToAction(nameof(Index));
        }

        //Export

        [HttpGet]
        public async Task<IActionResult> Export()
        {
            var csv = await _productService.ExportProducts();

            return File(
                csv,
                "text/csv",
                "products.csv");
        }

        //Import

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(
                    "file",
                    "Please select a CSV file.");

                return View();
            }

            try
            {
                await _productService.ImportProducts(file);

                TempData["Success"] =
                    "Products imported successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "file",
                    ex.Message);

                return View();
            }
        }
    }
}