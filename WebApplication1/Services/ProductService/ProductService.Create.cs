using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            await InvalidateProductCacheAsync();

            return product;
        }
    }
}
