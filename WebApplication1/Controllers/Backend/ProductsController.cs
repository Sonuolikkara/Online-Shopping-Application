using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using Serilog;

namespace WebApplication1.Properties.Controllers.Backend
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        //Testing Exception 
        [HttpGet("test-log")]
        public IActionResult TestLog()
        {
            Log.Information("Product test endpoint was called");

            return Ok(new
            {
                message = "Serilog test successful"
            });
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts();

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found. Try another product."
                });
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var createdProduct = await _productService.AddProduct(product);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = createdProduct.ProductID },
                createdProduct
            );
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest(new
                {
                    message = "Product ID does not match the URL ID."
                });
            }

            await _productService.UpdateProduct(product);

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProduct(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = $"Product with ID {id} not found. Try another product."
                });
            }

            return NoContent();
        }

        //Import

        [HttpPost("import")]
        public async Task<IActionResult> ImportProducts(IFormFile file)
        {
            await _productService.ImportProducts(file);

            return Ok(new
            {
                message = "Products imported successfully."
            });
        }

        //Export

        [HttpGet("export")]
        public async Task<IActionResult> ExportProducts()
        {
            var csvData = await _productService.ExportProducts();

            return File(
                csvData,
                "text/csv",
                "products.csv");
        }
    }
}