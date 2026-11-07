using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            try
            {
                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                await InvalidateProductCacheAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
