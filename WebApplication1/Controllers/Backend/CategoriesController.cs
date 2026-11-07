using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Backend
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetCategories();

            return Ok(categories);
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found. Try another category."
                });
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(
            Category category)
        {
            var createdCategory =
                await _categoryService.AddCategory(category);

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = createdCategory.CategoryID },
                createdCategory
            );
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            Category category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest(new
                {
                    message = "Category ID does not match the URL ID."
                });
            }

            await _categoryService.UpdateCategory(category);

            return NoContent();
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteCategory(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"Category with ID {id} not found. Try another category."
                });
            }

            return NoContent();
        }
    }
}
