using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        public async Task<Product> UpdateProduct(Product product)
        {
            var existingProduct = await _context.Products
                .FindAsync(product.ProductID);

            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.CategoryID = product.CategoryID;

            await _context.SaveChangesAsync();

            await InvalidateProductCacheAsync();

            return existingProduct;
        }
    }
}
