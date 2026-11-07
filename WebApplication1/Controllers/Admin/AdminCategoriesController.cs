using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminCategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public AdminCategoriesController(
            ICategoryService categoryService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        // GET: /AdminCategories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategories();

            return View(categories);
        }

        // GET: /AdminCategories/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /AdminCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryService.AddCategory(category);

            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminCategories/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: /AdminCategories/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryService.UpdateCategory(category);

            return RedirectToAction(nameof(Index));
        }

        //Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            var hasProducts =
                await _productService.HasProductsByCategoryId(id);

            ViewBag.HasProducts = hasProducts;

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            var hasProducts =
                await _productService.HasProductsByCategoryId(id);

            if (hasProducts)
            {
                TempData["Error"] =
                    "This category cannot be deleted because products are assigned to it.";

                return RedirectToAction(nameof(Index));
            }

            await _categoryService.DeleteCategory(id);

            TempData["Success"] =
                "Category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}